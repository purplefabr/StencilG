using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace StencilG
{
    public class GerberFile
    {
        private string filePath;
        private List<string> fileLines;                                         // Storing whole file in memory may not make sense later but it'll do for now
        private RegexOptions options;
        private List<IShape> fileShapes;

        //State variables
        private Unit fileUnit;
        private Dictionary<int, IShape> shapes;
        private int selectedShape;


        public GerberFile(string filePath)
        {
            this.filePath = filePath;
            this.fileLines = File.ReadAllLines(this.filePath).ToList();
            this.options = RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
            this.fileShapes = new List<IShape>();

            this.fileUnit = Unit.Inch;
            this.shapes = new Dictionary<int, IShape>();
            this.selectedShape = 0;

            
            foreach(string line in fileLines)
            {
                if (line.StartsWith("%MO"))
                {
                    if (line.Contains("IN"))
                    {
                        this.fileUnit = Unit.Inch;
                    }
                    else
                        throw new NotImplementedException();
                }
                else if(line.StartsWith("%ADD"))
                {
                    string expression = @"%ADD (?<index>\d+) R, (?<xDimension>[-+]?[0-9]*\.?[0-9]+) X (?<yDimension>[-+]?[0-9]*\.?[0-9]+) \*%";
                    Regex r = new Regex(expression, options);

                    Match match = r.Match(line);
                    if (!match.Success)
                        Console.WriteLine("Could not match {0}", line);
                    else
                    {
                        int index = Int32.Parse(match.Groups["index"].Value);
                        double x = double.Parse(match.Groups["xDimension"].Value);
                        double y = double.Parse(match.Groups["yDimension"].Value);
                        Rectangle rect = new Rectangle(new Point(0, 0), MathHelper.ToMillimeter(x), MathHelper.ToMillimeter(y), 0, 0.075);
                        shapes.Add(index, rect);
                    }
                }
                else if (line.StartsWith("G54"))
                {
                    //G54D10*
                    string expression = @"G54D (?<index>\d+)";
                    Regex r = new Regex(expression, options);
                    Match match = r.Match(line);
                    if (!match.Success)
                        Console.WriteLine("Could not match {0}", line);
                    else
                    {
                        int index = Int32.Parse(match.Groups["index"].Value);
                        selectedShape = index;
                    }
                }
                else if (line.StartsWith("X"))
                {
                    //X+1377Y+335D03*
                    string expression = @"X (?<xDimension>[-+]?[0-9]*\.?[0-9]+) Y (?<yDimension>[-+]?[0-9]*\.?[0-9]+) D (?<index>\d+)";
                    Regex r = new Regex(expression, options);

                    Match match = r.Match(line);
                    if (!match.Success)
                        Console.WriteLine("Could not match {0}", line);
                    else
                    {
                        int index = Int32.Parse(match.Groups["index"].Value);
                        if (index != 3)
                            throw new NotImplementedException();
                        
                        double x = ConvertNumberFormat(match.Groups["xDimension"].Value);
                        double y = ConvertNumberFormat(match.Groups["yDimension"].Value);
                        IShape shape = shapes[selectedShape];
                        fileShapes.Add(shape.CreateInstance(new Point(x, y)));
                        //Rectangle rect = new Rectangle(new Point(0, 0), x, y, 0, 0.075);
                        //shapes.Add(index, rect);
                    }
                }
            }
        }

        public List<ISegment> GetSegments()
        {
            List<ISegment> segments = new List<ISegment>();
            foreach(IShape shape in fileShapes)
            {
                segments.AddRange(shape.Segments);
            }
            return segments;
        }

        private double ConvertNumberFormat(string number)
        {
            return MathHelper.ToMillimeter(double.Parse(number)/10000);
        }
    }

    public enum Unit
    {
        Inch,
        Millimeter
    }
}
