using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class Point
    {
        public double X { get; internal set; }
        public double Y { get; internal set; }

        public Point() { }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void Update(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void Update(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
    }
}
