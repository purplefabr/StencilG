using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class Cutter
    {
        private StreamWriter streamWriter;              // Streamwriter for output stream
        private bool headingKnown;                      // False when blade direction hasn't been initialised
        private double heading;                         // Blade direction in degrees
        private double zHeight;                         // if zHeight = 0 then cutter is cutting, so no plunge required
        private string commentColumn;                   // Position of all gcode comments, make this larger than longest gcode length
        private double xScratchOrigin;                  // Position of scratch area 0,y (delta printers have machine origin in the middle so set to negative value)
        private double yScratchOrigin;                  // Position of scratch area x,0 (delta printers have machine origin in the middle so set to negative value)       
        private Point cutterPoint;                      // XY position of the cutting point
        private Point toolPoint;                        // XY position of the tool centre
        private bool homed;                             // Indicates that cutter has been homed

        public double CutterDiameter { get; set; }      // Blade body diameter in mm
        public double CutterAngle { get; set; }         // Blade cutting angle in degrees
        public double MoveHeight { get; set; }          // Z height to do moves at (where tool is clear of work area)
        public double CutHeight { get; set; }           // Z height to cut at (where tool is engaged with material)
        public double MoveSpeed { get; set; }           // Speed (mm/min) to do moves at
        public double CutSpeed { get; set; }            // Speed (mm/min) to do cuts at
        public double ToolZSpeed { get; set; }          // Speed (mm/min) to plunge and retract tool at
        public bool EnableGCodeComments { get; set; }   // Output comments to gcode file
        private double preStartDistance;
        public double PreStartDistance                  // Offsets cutter by this distance before a cut to make corners sharper
        {
            get
            {
                return preStartDistance;
            }
            set
            {
                if (value > (CutterDiameter / 2))
                    throw new CutterException("Could not set PreStartDistance; distance larger than CutterDiameter/2");
                else
                    preStartDistance = value;
            }
        }
        public double PostEndDistance { get; set; }

        public Cutter(StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;
            this.headingKnown = false;
            this.heading = 0;
            this.zHeight = double.PositiveInfinity;
            this.commentColumn = "40";
            this.xScratchOrigin = -30;
            this.yScratchOrigin = -30;
            this.cutterPoint = new Point(double.NaN, double.NaN);
            this.toolPoint = new Point(double.NaN, double.NaN);
            this.preStartDistance = 0;
            this.homed = false;

            CutterDiameter = 0.9;
            CutterAngle = 45;
            MoveHeight = 5;
            CutHeight = 0;
            MoveSpeed = 600;
            CutSpeed = 60;
            ToolZSpeed = 50;
            EnableGCodeComments = true;
        }

        public void Render(LineSegment segment)
        {
            if (!homed) {
                Home();
                Start();
            }
            if (!this.headingKnown || (this.heading != segment.Heading))
            {
                OrientCutter(segment);
            }

            Move(segment, "Move");
            Plunge();
            Cut(segment, "Cut");
            Retract();
        }

        #region GCode Helpers

        public void Home()
        {
            Write("G28", "Home printer");
            this.zHeight = double.PositiveInfinity;  //At max Z due to G28 home
            this.toolPoint.X = 0;
            this.toolPoint.Y = 0;
            if (!headingKnown)
            {
                this.cutterPoint.Update(double.NaN, double.NaN);
            }
            else
            {
                throw new NotImplementedException();
            }
            homed = true;
        }

        private void Start()
        {
            Write("G1 X" + GCodeDouble(0) + " Y" + GCodeDouble(0) + " Z" + GCodeDouble(MoveHeight) + " F" + GCodeDouble(MoveSpeed * 2), "Move to start position");
            this.zHeight = MoveHeight;
            this.toolPoint.Update(0, 0);
            if (!headingKnown)
            {
                this.cutterPoint.Update(double.NaN, double.NaN);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Move(LineSegment segment, string comment, bool ignoreCutterDiameter = false)
        {
            if (this.zHeight != MoveHeight)
                throw new CutterException("Lockout on Move due to incorrect Z Height");

            var preMovePoint = MathHelper.CalcEndPoint(segment.Start, (segment.Heading + 180) % 360, 10);
            Write("G1 X" + GCodeDouble(preMovePoint.X) + " Y" + GCodeDouble(preMovePoint.Y) + " F" + GCodeDouble(MoveSpeed), "PreMove point");

            double distance;
            if (ignoreCutterDiameter)
                distance = 0;
            else
                distance = (CutterDiameter / 2) - preStartDistance;

            var toolPoint = MathHelper.CalcEndPoint(segment.Start, segment.Heading, distance);
            

            Write("G1 X" + GCodeDouble(toolPoint.X) + " Y" + GCodeDouble(toolPoint.Y) + " F" + GCodeDouble(MoveSpeed), comment);
            this.toolPoint.Update(toolPoint);
            this.cutterPoint.Update(segment.Start);
        }

        private void Cut(LineSegment segment, string comment, bool ignoreCutterDiameter = false)
        {
            if ((segment.Start.X != this.cutterPoint.X) || (segment.Start.Y != this.cutterPoint.Y))
                throw new CutterException("Lockout on Cut due to incorrect cutter position");
            if (this.zHeight != CutHeight)
                throw new CutterException("Lockout on Cut due to incorrect Z Height");

            double h2;
            if (ignoreCutterDiameter)
                h2 = segment.Length;
            else
                h2 = segment.Length + (CutterDiameter / 2) + PostEndDistance;

            var toolEndPoint = MathHelper.CalcEndPoint(segment.Start, segment.Heading, h2);

            Write("G1 X" + GCodeDouble(toolEndPoint.X) + " Y" + GCodeDouble(toolEndPoint.Y) + " F" + GCodeDouble(CutSpeed), comment);
            this.toolPoint.Update(toolEndPoint);
            this.cutterPoint.Update(segment.End);
        }

        private void Plunge()
        {
            if (this.zHeight != CutHeight)
                Write("G1 Z" + GCodeDouble(CutHeight) + " F" + GCodeDouble(ToolZSpeed), "Plunge blade");
            else
                throw new CutterException("Lockout on Plunge due to already being at CutHeight");
            this.zHeight = CutHeight;
        }

        private void Retract()
        {
            if(this.zHeight != MoveHeight)
                Write("G1 Z" + GCodeDouble(MoveHeight) + " F" + GCodeDouble(ToolZSpeed), "Retract blade");
            else
                throw new CutterException("Lockout on Retract due to already being at MoveHeight");
            this.zHeight = MoveHeight;
        }

        private void OrientCutter(LineSegment segment)
        {
            LineSegment orientSegment = new LineSegment(new Point(xScratchOrigin, yScratchOrigin), segment.Heading, 5);

            Move(orientSegment, "Move to cutter orientation area", true);
            Plunge();
            Cut(orientSegment, "Orient cutter to heading of " + GCodeDouble(segment.Heading), true);
            Retract();
            this.heading = orientSegment.Heading;
            this.headingKnown = true;
        }

        #endregion

        private string GCodeDouble(double value)
        {
            return string.Format("{0:F2}", value);
        }

        private void Write(string command, string comment)
        {
            if ((comment != null) && (comment != "") && EnableGCodeComments)
                streamWriter.WriteLine(String.Format("{0,-" + commentColumn + "}; {1}", command, comment));
            else
                streamWriter.WriteLine(command);
        }
    }
}
