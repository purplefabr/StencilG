using StencilG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StencilGApp
{
    public static class CanvasExtensionMethods
    {
        public static void LineSegment(this Canvas canvas, StencilG.LineSegment lineSegment, double scale)
        {
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            Path path = new Path();
            path.Stroke = blackBrush;
            path.StrokeThickness = 1;

            LineGeometry line = new LineGeometry();
            line.StartPoint = new System.Windows.Point(lineSegment.Start.X * scale, lineSegment.Start.Y * scale);
            line.EndPoint = new System.Windows.Point(lineSegment.End.X * scale, lineSegment.End.Y * scale);

            path.Data = line;
            canvas.Children.Add(path);
        }
    }
}
