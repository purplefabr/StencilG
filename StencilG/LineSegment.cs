using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class LineSegment
    {
        public Point Start { get; set; }
        public Point End { get; set; }
        public double dX { get; set; }
        public double dY { get; set; }
        public double Length { get; set; }
        public double Heading { get; set; }
        public double HeadingRad { get; set; }
        public LineSegmentType Type { get; set; }

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

    public enum LineSegmentType
    {
        Move,
        Score,
        Cut
    }
}
