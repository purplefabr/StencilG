using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilGApp
{
    public class GCode
    {
        string Command;
        double X;
        double Y;
        double Z;
        double F;

        public GCode()
        {
            Command = "G1";
            X = double.NaN;
            Y = double.NaN;
            Z = double.NaN;
            F = 100;
        }

        public GCode(string command, double x, double y, double z, double f)
        {
            Command = command;
            X = x;
            Y = y;
            Z = z;
            F = f;
        }

        public GCode(GCode gcode, double offsetX, double offsetY)
        {
            Command = gcode.Command;
            if (gcode.X != double.NaN)
                X = gcode.X + offsetX;
            else
                X = double.NaN;
            if (gcode.Y != double.NaN)
                Y = gcode.Y + offsetY;
            else
                Y = double.NaN;
            Z = gcode.Z;
            F = gcode.F;
        }

        public GCode(string command)
        {
            Command = command;
            X = double.NaN;
            Y = double.NaN;
            Z = double.NaN;
            F = double.NaN;
        }

        public override string ToString()
        {
            string data = "";
            data += Command + " ";
            if(X != double.NaN)
            {
                data += "X" + X.ToString("F4") + " ";
            }
            if(Y != double.NaN)
            {
                data += "Y" + Y.ToString("F4") + " ";
            }
            if(Z != double.NaN)
            {
                data += "Z" + Z.ToString("F4") + " ";
            }
            if (F != double.NaN)
            {
                data += "F" + F.ToString("F1");
            }
            return data;
                   // "G1 X" + printerPoint.X.ToString() + " Y" + printerPoint.Y.ToString() + " F50000"
        }
    }
}
