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
        private double cutterX, cutterY;            // XY position of the cutting point
        private double toolX, toolY;                // XY position of the tool centre

        public double CutterDiameter { get; set; }  // Blade body diameter in mm
        public double CutterAngle { get; set; }     // Blade cutting angle in degrees
        public double MoveHeight { get; set; }      // Z height to do moves at (where tool is clear of work area)
        public double CutHeight { get; set; }       // Z height to cut at (where tool is engaged with material)
        public double MoveSpeed { get; set; }       // Speed (mm/min) to do moves at
        public double CutSpeed { get; set; }        // Speed (mm/min) to do cuts at
        public double ToolZSpeed { get; set; }      // Speed (mm/min) to plunge and retract tool at
        public double RotateSpeed { get; set; }
        public bool EnableGCodeComments { get; set; }

        public Cutter(StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;
            this.headingKnown = false;
            this.heading = 0;
            this.zHeight = double.PositiveInfinity;
            this.commentColumn = "50";
            this.xWorkOrigin = 0;
            this.yWorkOrigin = 0;

            CutterDiameter = 0.9;
            CutterAngle = 45;
            MoveHeight = 10;
            CutHeight = 0;
            MoveSpeed = 200;
            CutSpeed = 20;
            ToolZSpeed = 20;
            RotateSpeed = 5;
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
            this.toolX = 0;
            this.toolY = 0;
        }

        private void Start()
        {
            Write("G1 X" + GCodeDouble(0) + " Y" + GCodeDouble(0) + " Z" + GCodeDouble(MoveHeight) + " F" + GCodeDouble(MoveSpeed), "Move to start position");
            this.zHeight = MoveHeight;
            this.toolX = 0;
            this.toolY = 0;
        }

        private void Move(double cutterX, double cutterY, string comment)
        {
            if (this.zHeight == MoveHeight)
            {
                Write("G1 X" + GCodeDouble(cutterX) + " Y" + GCodeDouble(cutterY) + " F" + GCodeDouble(MoveSpeed), comment);
                //this.toolX = x;
                //this.toolY = y;
            }
            else
                throw new CutterException("Lockout on Move due to incorrect Z Height");           
        }

        private void Cut(double cutterX, double cutterY, string comment){
            if (this.zHeight == CutHeight)
            {
                double xDifference = cutterX - this.cutterX;
                double yDifference = cutterY - this.cutterY;

                //Calculate heading
                double headingRad = Math.Atan2(xDifference, yDifference);
                if (headingRad < 0)
                    headingRad += 2 * Math.PI;
                double headingDeg = headingRad * (180 / Math.PI);

                if (headingDeg != this.heading)
                {
                    //Rotate cutter
                    //Arc()
                    //this.heading = headingDeg;
                }

                //Calculate length of hypotenuse
                double h = Math.Sqrt(Math.Pow(Math.Abs(xDifference), 2) + Math.Pow(Math.Abs(yDifference), 2));
                double h2 = h + CutterDiameter / 2;

                //double toolX = Math.Sin(headingRad)

                double toolX = cutterX;
                double toolY = cutterY;

                Write("G1 X" + GCodeDouble(toolX) + " Y" + GCodeDouble(toolY) + " F" + GCodeDouble(CutSpeed), comment);
                this.toolX = x;
                this.toolY = y;
            }
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

        private void Arc(double newHeading)
        {
            if (CW)
                Write("G2 X" + GCodeDouble(finishX) + " Y" + GCodeDouble(finishY) + " I" + GCodeDouble(cutterX) + " J" + GCodeDouble(cutterY) + " F" + GCodeDouble(RotateSpeed), "Rotating blade");
            else
                Write("G3 X" + GCodeDouble(finishX) + " Y" + GCodeDouble(finishY) + " I" + GCodeDouble(cutterX) + " J" + GCodeDouble(cutterY) + " F" + GCodeDouble(RotateSpeed), "Rotating blade");
            this.toolX = finishX;
            this.toolY = finishY;
        }

        private void OrientCutter(double heading)
        {
            if (this.headingKnown)
                throw new CutterException("Heading is known");
            if (heading != 0)
                throw new NotImplementedException();
            Move(xWorkOrigin, yWorkOrigin, "Move to cutter orientation area");
            //Plunge();
            //Cut(xWorkOrigin, yWorkOrigin + 10, "Orient cutter to heading of " + GCodeDouble(heading));
            Move(xWorkOrigin -10, yWorkOrigin + 10, "Test");
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

            int difference = (int)(newHeading - this.heading);

            //switch(difference)
            //{
            //    case 0:
            //        break;
            //    case -90:       //CCW --> to ^
            //        Arc()

            //}

            //Generate G2/G3 move to orient blade

        }
    }
}
