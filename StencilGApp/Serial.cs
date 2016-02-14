using System;
using System.IO.Ports;

namespace StencilGApp
{
    public class Serial
    {
        private SerialPort _comPort = new SerialPort();
        public bool IsOpen { get; set; }

        public Serial(string PortName)
        {
            _comPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);

            _comPort.PortName = Convert.ToString(PortName);
            _comPort.BaudRate = 115200;
            _comPort.DataBits = 8;
            _comPort.StopBits = StopBits.One;
            _comPort.Handshake = Handshake.None;
            _comPort.Parity = Parity.None;
            try
            {
                _comPort.Open();
            }
            catch (Exception e)
            {
                // System.Windows.Forms.MessageBox.Show("Exception: " + e.ToString());
                //Console.WriteLine(e.ToString()); 
            }

            if (_comPort.IsOpen)
            {
                IsOpen = true;
            }
            else
            {
                IsOpen = false;
            }
        }

        public void Close()
        {
            _comPort.Close();
            if (_comPort.IsOpen)
            {
                IsOpen = true;
            }
            else
            {
                IsOpen = false;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string inputData = _comPort.ReadExisting();
            //if (inputData != String.Empty)
            //{
            //    switch (inputData)
            //    {
            //        case "N":
            //            inputData = "0";
            //            break;
            //        case "R":
            //            inputData = "8";
            //            break;
            //        case "P":
            //            inputData = "7";
            //            break;
            //    }
            //    QueueManager.Instance.TransmitQueue.Add(new PSXData("Qh413", inputData));
            //}
        }

        public void Send(string data)
        {
            if (IsOpen)
            {
                _comPort.Write(data + "\r");
            }
        }
    }
}
