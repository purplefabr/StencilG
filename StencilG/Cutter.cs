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
        private StreamWriter streamWriter;          // Streamwriter for output stream
        private bool headingKnown;                  // False when blade direction hasn't been initialised
        private double heading;                     // Blade direction in degrees
        private double zHeight;                     // if zHeight = 0 then cutter is cutting, so no plunge required
        private string commentColumn;               // Position of all gcode comments, make this larger than longest gcode length
        private double xWorkOrigin;                 // Position of 0,y (delta printers have machine origin in the middle so set to negative value)
        private double yWorkOrigin;                 // Position of x,0 (delta printers have machine origin in the middle so set to negative value)           

        public double CutterDiameter { get; set; }  // Blade body diameter in mm
        public double CutterAngle { get; set; }     // Blade cutting angle in degrees
        public double MoveHeight { get; set; }      // Z height to do moves at (where tool is clear of work area)
        public double CutHeight { get; set; }       // Z height to cut at (where tool is engaged with material)
        public double MoveSpeed { get; set; }       // Speed (mm/min) to do moves at
        public double CutSpeed { get; set; }        // Speed (mm/min) to do cuts at
        public double ToolZSpeed { get; set; }      // Speed (mm/min) to plunge and retract tool at
        public bool EnableGCodeComments { get; set; }

        public Cutter(StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;
            this.headingKnown = false;
            this.heading = 0;
            this.zHeight = double.PositiveInfinity;
            this.commentColumn = "50";
            this.xWorkOrigin = -30;
            this.yWorkOrigin = -30;

            CutterDiameter = 0.9;
            CutterAngle = 45;
            MoveHeight = 10;
            CutHeight = 0;
            MoveSpeed = 200;
            CutSpeed = 20;
            ToolZSpeed = 20;
            EnableGCodeComments = true;

            Home();
            Start();
            OrientCutter(0);
        }

        public void Render(LineSegment segment)
        {
            if (!this.headingKnown)
            {
                OrientCutter(0);
            }
        }

        #region GCode Helpers

        private void Home()
        {
            Write("G28", "Home printer");
            this.zHeight = double.PositiveInfinity;  //At max Z due to G28 home
        }

        private void Start()
        {
            Write("G1 X" + GCodeDouble(0) + " Y" + GCodeDouble(0) + " Z" + GCodeDouble(MoveHeight) + " F" + GCodeDouble(MoveSpeed), "Move to start position");
            this.zHeight = MoveHeight;
        }

        private void Move(double x, double y, string comment)
        {
            if (this.zHeight == MoveHeight)
                Write("G1 X" + GCodeDouble(x) + " Y" + GCodeDouble(y) + " F" + GCodeDouble(MoveSpeed), comment);
            else
                throw new CutterException("Lockout on Move due to incorrect Z Height");           
        }

        private void Cut(double x, double y, string comment){
            if(this.zHeight == CutHeight)
                Write("G1 X" + GCodeDouble(x) + " Y" + GCodeDouble(y) + " F" + GCodeDouble(CutSpeed), comment);
            else
                throw new CutterException("Lockout on Cut due to incorrect Z Height");
        }

        private void Plunge(){
            Write("G1 Z" + GCodeDouble(CutHeight) + " F" + GCodeDouble(ToolZSpeed), "Plunge blade");
            this.zHeight = CutHeight;
        }

        private void Retract()
        {
            Write("G1 Z" + GCodeDouble(MoveHeight) + " F" + GCodeDouble(ToolZSpeed), "Retract blade");
            this.zHeight = MoveHeight;
        }

        private void OrientCutter(double heading)
        {
            if (this.headingKnown)
                throw new CutterException("Heading is known");
            if (heading != 0)
                throw new NotImplementedException();
            Move(xWorkOrigin, yWorkOrigin, "Move to cutter orientation area");
            Plunge();
            Cut(xWorkOrigin, yWorkOrigin + 10, "Orient cutter to heading of " + GCodeDouble(heading));
            Retract();
            this.heading = heading;
            this.headingKnown = true;
        }

        #endregion

        private string GCodeDouble(double value)
        {
            return string.Format("{0:N2}", value);
        }

        private void Write(string command, string comment)
        {
            if ((comment != null) && (comment != "") && EnableGCodeComments)
                streamWriter.WriteLine(String.Format("{0,-" + commentColumn + "}; {1}", command, comment));
            else
                streamWriter.WriteLine(command);
        }      

        private void ChangeHeading(double newHeading)
        {
            if (!headingKnown)
                throw new CutterException("Current heading is unknown");

            //Generate G2/G3 move to orient blade

        }
    }
}
