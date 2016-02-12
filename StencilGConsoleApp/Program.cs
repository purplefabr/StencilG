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
            GerberFile input = new GerberFile("C:\\Users\\Master\\Documents\\TestSMT.ger");
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            using(StreamWriter sw = new StreamWriter(fs)){
                Cutter cutter = new Cutter(sw);
                //cutter.PreStartDistance = 0.2;
                cutter.PostEndDistance = 0.175;
                cutter.MoveHeight = 3;
                cutter.CutterDiameter = 1;
                
                List<ISegment> segments = new List<ISegment>();

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
