using System;
using System.Windows;
using System.IO.Ports;

namespace StencilGApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller = new Controller();
        ControlWindow controlWindow = new ControlWindow();

        public MainWindow()
        {
            InitializeComponent();
            controller.Canvas = this.canvasWorkArea;
            controller.Scale = 10;
        }

        private void btnLoadGerber_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".ger";
            dlg.Filter = "Gerber|*.ger";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                controller.InputPath = dlg.FileName;
            }


        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".gcode";
            dlg.Filter = "GCode|*.gcode";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                controller.OutputPath = dlg.FileName;
                controller.Convert();
            }


        }

        private void btnControl_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window n in Application.Current.Windows)
                if (n.Name == "controlWindow")
                { }
                else
                { controlWindow.Show(); }

            controlWindow.Activate();
        }
    }
}
