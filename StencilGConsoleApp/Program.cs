using StencilG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StencilGConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath), "test.gcode");
            using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            using(StreamWriter sw = new StreamWriter(fs)){
                Cutter cutter = new Cutter(sw);
                cutter.PreStartDistance = 0.2;
                List<LineSegment> segments = new List<LineSegment>();
                segments.AddRange(new Square(new Point(0.5, 0.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(2.5, 0.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(4.5, 0.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(0.5, 2.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(2.5, 2.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(4.5, 2.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(0.5, 4.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(2.5, 4.5), 1, 0).Segments);
                segments.AddRange(new Square(new Point(4.5, 4.5), 1, 0).Segments);
                segments = segments.OrderBy(s => s.Heading).ToList();
                foreach(LineSegment segment in segments)
                {
                    cutter.Render(segment);
                }
            }
        }
    }
}
