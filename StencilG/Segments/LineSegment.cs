using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class LineSegment : ISegment
    {
        public Point Start { get; internal set; }
        public Point End { get; internal set; }
        public double dX { get; internal set; }
        public double dY { get; internal set; }
        public double Length { get; internal set; }
        public double Heading { get; internal set; }
        public double HeadingRad { get; internal set; }

        public LineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
            dX = End.X - Start.X;
            dY = End.Y - Start.Y;

            // Calculate Heading
            double headingRad = Math.Atan2(dX, dY);
            if (headingRad < 0)
                headingRad += 2 * Math.PI;
            double headingDeg = headingRad * (180 / Math.PI);
            Heading = headingDeg;
            HeadingRad = headingRad;

            // Calculate Length
            Length = Math.Sqrt(Math.Pow(Math.Abs(dX), 2) + Math.Pow(Math.Abs(dY), 2));
        }

        public LineSegment(Point start, double heading, double length)
        {
            Start = start;
            End = MathHelper.CalcEndPoint(start, heading, length);
            Heading = heading;
            Length = length;
        }
    }
}
