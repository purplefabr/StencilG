using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class Square     //Square is subset of rectangle, deprecate?
    {
        public List<LineSegment> Segments { get; set; }
        public Square(Point origin, double width, double rotation)
        {
            if (rotation != 0)
                throw new NotImplementedException();
            Segments = new List<LineSegment>();
            Segments.Add(new LineSegment(new Point(origin.X,            origin.Y            ), new Point(origin.X + width,  origin.Y            )));
            Segments.Add(new LineSegment(new Point(origin.X,            origin.Y + width    ), new Point(origin.X + width,  origin.Y + width    )));
            Segments.Add(new LineSegment(new Point(origin.X,            origin.Y            ), new Point(origin.X,          origin.Y + width    )));
            Segments.Add(new LineSegment(new Point(origin.X + width,    origin.Y            ), new Point(origin.X + width,  origin.Y + width    )));
        }
    }
}
