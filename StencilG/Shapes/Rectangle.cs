using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilG
{
    public class Rectangle : IShape
    {
        public List<ISegment> Segments { get; internal set; }
        public Point Centre { get; internal set; }
        public double Rotation { get; internal set; }
        public double Shrink { get; internal set; }

        public double Width { get; internal set; }
        public double Height { get; internal set; }

        public Rectangle(Point centre, double width, double height, double rotation, double shrink = 0)
        {
            if (rotation != 0)
                throw new NotImplementedException();
            Centre = centre;
            Rotation = rotation;
            Shrink = shrink;
            Width = width;
            Height = height;
            CalculateSegments();
        }

        public IShape CreateInstance(Point origin)
        {
            return new Rectangle(origin, Width, Height, Rotation, Shrink);
        }

        private void CalculateSegments()
        {
            if (Segments == null)
                Segments = new List<ISegment>();
            else
                Segments.Clear();
            //       2
            //   x-------x
            //   |       |
            // 3 |   o   | 4
            //   |       |
            //   x-------x
            //       1
            Segments.Add(new LineSegment(
                new Point(Centre.X - (Width / 2) + (Shrink / 2),    Centre.Y - (Height / 2) + (Shrink / 2)), 
                new Point(Centre.X + (Width / 2) - (Shrink / 2),    Centre.Y - (Height / 2) + (Shrink / 2))));
            Segments.Add(new LineSegment(
                new Point(Centre.X - (Width / 2) + (Shrink / 2),    Centre.Y + (Height / 2) - (Shrink / 2)), 
                new Point(Centre.X + (Width / 2) - (Shrink / 2),    Centre.Y + (Height / 2) - (Shrink / 2))));
            Segments.Add(new LineSegment(
                new Point(Centre.X - (Width / 2) + (Shrink / 2),    Centre.Y - (Height / 2) + (Shrink / 2)), 
                new Point(Centre.X - (Width / 2) + (Shrink / 2),    Centre.Y + (Height / 2) - (Shrink / 2))));
            Segments.Add(new LineSegment(
                new Point(Centre.X + (Width / 2) - (Shrink / 2), Centre.Y - (Height / 2) + (Shrink / 2)), 
                new Point(Centre.X + (Width / 2) - (Shrink / 2), Centre.Y + (Height / 2) - (Shrink / 2))));
        }
    }
}
