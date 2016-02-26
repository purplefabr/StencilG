using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using System.Windows.Input;

namespace StencilGApp
{
    /// <summary>
    /// Interaction logic for ControlWindow.xaml
    /// </summary>
    public partial class ControlWindow : Window
    {
        private Serial serial;
        private bool isDrag = false;
        private bool isConnected = false;
        private double Z;
        private List<GCode> recordList = new List<GCode>();
        private bool recording = false;

        public ControlWindow()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            serial = new Serial((string)cmbComPort.SelectedItem);
            isConnected = serial.IsOpen;
            serial.Send("G28");
            serial.Send("G1 Z30 F5000");
            Z = 30;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDrag = true;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                isDrag = true;
                recordList.Clear();
                recording = true;
                serial.Send("G1 Z30 F5000");
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrag = false;
            recording = false;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {

                //Point endPoint = ((Control)sender).PointToScreen(e.GetPosition());

                //int width = endPoint.X - startPoint.X;
                //int height = endPoint.Y - startPoint.Y;

                //var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                //var mouse = transform.Transform(GetMousePosition());
                var mouse = e.GetPosition(border);
                Point printerPoint = new Point((mouse.X - (border.ActualWidth / 2)) / 10, (-mouse.Y + (border.ActualHeight / 2)) / 10);
                label1.Content = printerPoint.X.ToString();
                label2.Content = printerPoint.Y.ToString();

                GCode gcode = new GCode("G1", printerPoint.X, printerPoint.Y, double.NaN, 50000);
                RenderGCode(gcode);
            }
        }

        public Point GetMousePosition()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new System.Windows.Point(point.X, point.Y);
        }

        private void controlWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                cmbComPort.Items.Add(s);
            }
        }

        private void border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Z += ((float)e.Delta) / 300;
            GCode gcode = new GCode("G1", double.NaN, double.NaN, Z, 50000);
            RenderGCode(gcode);
        }

        private void border_DragLeave(object sender, DragEventArgs e)
        {
            isDrag = false;
            recording = false;
        }

        private void controlWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.H:
                    serial.Send("G28");
                    serial.Send("G1 Z30 F5000");
                    Z = 30;
                    break;
                case Key.P:
                    serial.Send("G28");
                    serial.Send("G1 Z30 F5000");
                    foreach (GCode gcode in recordList)
                    {
                        serial.Send(new GCode(gcode, sldXOffset.Value, sldYOffset.Value).ToString());
                    }
                    break;
                case Key.G:

                    break;
            }
        }

        private void RenderGCode(GCode gcode)
        {
            serial.Send(gcode.ToString());
            if (recording)
                recordList.Add(gcode);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridGenerator grid = new GridGenerator(5, 5, 20, 20);
            grid.BottomLeftOrigin = new StencilG.Point(-50, -50);
            grid.Compute();
            foreach (GCode gcode in grid.Commands)
            {
                serial.Send(gcode.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GridGenerator grid = new GridGenerator(25, 25, 3, 3);
            grid.BottomLeftOrigin = new StencilG.Point(-37.5, -37.5);
            List<GCode> commands = grid.RepeatPattern(recordList);
            foreach (GCode gcode in commands)
            {
                serial.Send(gcode.ToString());
            }
        }
    }
}
