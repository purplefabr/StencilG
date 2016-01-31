using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class MathHelper
    {
        public static Point CalcEndPoint(Point start, double heading, double distance)
        {
            double correctedX = 0;
            double correctedY = 0;
            if ((heading % 90) == 0)
            {
                int factor = (int)heading;
                switch (factor)
                {
                    case 0:
                    case 180:
                        correctedY = distance;
                        correctedX = 0;
                        break;
                    case 90:
                    case 270:
                        correctedY = 0;
                        correctedX = distance;
                        break;

                }
            }
            else
            {
                double headingRad = heading * (Math.PI / 180);
                correctedX = Math.Sin(headingRad % (Math.PI / 2)) * distance;
                correctedY = Math.Cos(headingRad % (Math.PI / 2)) * distance;
            }

            double xSign, ySign;
            if (heading <= 90)
            {
                xSign = 1;
                ySign = 1;
            }
            else if (heading <= 180)
            {
                xSign = 1;
                ySign = -1;
            }
            else if (heading <= 270)
            {
                xSign = -1;
                ySign = -1;
            }
            else
            {
                xSign = -1;
                ySign = 1;
            }

            Point point = new Point();
            point.X = start.X + (correctedX * xSign);
            point.Y = start.Y + (correctedY * ySign);
            return point;
        }

        public static double ToMillimeter(double inch)
        {
            return inch / 0.0393700787402;
        }
    }
}
