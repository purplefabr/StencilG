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
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrag = false;
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
                Point printerPoint = new Point((mouse.X - (border.ActualWidth / 2)) / 20, (-mouse.Y + (border.ActualHeight / 2)) / 20);
                label1.Content = printerPoint.X.ToString();
                label2.Content = printerPoint.Y.ToString();
                serial.Send("G1 X" + printerPoint.X.ToString() + " Y" + printerPoint.Y.ToString() + " F50000");
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
            Z += ((float)e.Delta)/300;
            serial.Send("G1 Z" + Z.ToString());
        }
    }
}
