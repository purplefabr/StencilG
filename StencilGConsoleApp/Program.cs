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
                
            }
        }
    }
}
