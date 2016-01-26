using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class LineSegment
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Heading { get; set; }
        public LineSegmentType Type { get; set; }
    }

    public enum LineSegmentType
    {
        Move,
        Score,
        Cut
    }
}
