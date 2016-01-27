using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class Square
    {
        public List<LineSegment> Segments { get; set; }
        public Square(Point center, double width, double rotation)
        {
            if (rotation != 0)
                throw new NotImplementedException();
            Segments = new List<LineSegment>();
            Segments.Add(new LineSegment(new Point(center.X - (width / 2), center.Y - (width / 2)), new Point(center.X + (width / 2), center.Y - (width / 2))));
            Segments.Add(new LineSegment(new Point(center.X - (width / 2), center.Y + (width / 2)), new Point(center.X + (width / 2), center.Y + (width / 2))));
            Segments.Add(new LineSegment(new Point(center.X - (width / 2), center.Y - (width / 2)), new Point(center.X - (width / 2), center.Y + (width / 2))));
            Segments.Add(new LineSegment(new Point(center.X + (width / 2), center.Y - (width / 2)), new Point(center.X + (width / 2), center.Y + (width / 2))));
        }
    }
}
