using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StencilG;
using System.Windows.Controls;

namespace StencilGApp
{
    public class Controller
    {
        public string InputPath { get; set; }
        public string OutputPath { get; set; }      //"C:\\Users\\Master\\Documents\\TestSMT.ger"
        public Canvas Canvas { get; set; }
        public double Scale { get; set; }

        public void Convert()
        {
            GerberFile input = new GerberFile(InputPath);
            using(FileStream fs = new FileStream(OutputPath, FileMode.Create, FileAccess.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                Cutter cutter = new Cutter(sw);
                //cutter.PreStartDistance = 0.2;
                cutter.PostEndDistance = 0.175;
                cutter.MoveHeight = 3;
                cutter.CutterDiameter = 1;

                List<ISegment> segments = new List<ISegment>();

                segments = input.GetSegments();

                segments = segments.OrderBy(s => s.Heading).ToList();
                foreach (LineSegment segment in segments)
                {
                    cutter.Render(segment);
                    Canvas.LineSegment(segment, Scale);
                }
            }
        }
    }
}
