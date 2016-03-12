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
            DrawLine(canvas, lineSegment.Start.X * scale, lineSegment.Start.Y * scale, lineSegment.End.X * scale, lineSegment.End.Y * scale, Colors.Black);
        }

        public static void CenterCrosshair(this Canvas canvas)
        {
            DrawLine(canvas, -5, 0, 5, 0, Colors.Red);
            DrawLine(canvas, 0, -5, 0, 5, Colors.Red);
        }

        public static void PartCrosshair(this Canvas canvas, double x, double y, double scale)
        {
            DrawLine(canvas, (x * scale) - 5, y * scale, (x * scale) + 5, y * scale, Colors.Green);
            DrawLine(canvas, x * scale, (y * scale) - 5, x * scale, (y * scale) + 5, Colors.Green);
        }

        private static void DrawLine(Canvas canvas, double startX, double startY, double endX, double endY, Color colour)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = colour;

            Path path = new Path();
            path.Stroke = brush;
            path.StrokeThickness = 1;

            LineGeometry line = new LineGeometry();
            line.StartPoint = new System.Windows.Point(startX, startY);
            line.EndPoint = new System.Windows.Point(endX, endY);

            path.Data = line;
            canvas.Children.Add(path);
        }
    }
}
