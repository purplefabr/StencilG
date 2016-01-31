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
            GCodeFile input = new GCodeFile("C:\\Users\\Master\\Documents\\input.ger");
            using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            using(StreamWriter sw = new StreamWriter(fs)){
                Cutter cutter = new Cutter(sw);
                //cutter.PreStartDistance = 0.2;
                cutter.PostEndDistance = 0.175;
                cutter.MoveHeight = 3;
                cutter.CutterDiameter = 1;
                
                List<ISegment> segments = new List<ISegment>();
                //segments.AddRange(new Square(new Point(0.25, 0.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(1.25, 0.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(2.25, 0.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(0.25, 1.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(1.25, 1.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(2.25, 1.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(0.25, 2.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(1.25, 2.25), 0.5, 0).Segments);
                //segments.AddRange(new Square(new Point(2.25, 2.25), 0.5, 0).Segments);

                //MSOP maybe? Might be a little small
                //segments.AddRange(new Rectangle(new Point(0, 0), 1.8, 0.25, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(0, 0.5), 1.8, 0.25, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(0, 1), 1.8, 0.25, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(0, 1.5), 1.8, 0.25, 0, 0.075).Segments);

                //segments.AddRange(new Rectangle(new Point(5.2, 0), 1.8, 0.25, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 0.5), 1.8, 0.25, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 1), 1.8, 0.25, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 1.5), 1.8, 0.25, 0, 0.075).Segments);

                //SOIC8
                //segments.AddRange(new Rectangle(new Point(0, 0), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(0, 1.27), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(0, 2.54), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(0, 3.81), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 0), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 1.27), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 2.54), 2.2, 0.6, 0, 0.075).Segments);
                //segments.AddRange(new Rectangle(new Point(5.2, 3.81), 2.2, 0.6, 0, 0.075).Segments);

                segments = input.GetSegments();

                segments = segments.OrderBy(s => s.Heading).ToList();
                foreach(LineSegment segment in segments)
                {
                    cutter.Render(segment);
                }
            }
        }
    }
}
