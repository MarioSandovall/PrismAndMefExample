using Prism.Events;
using Shared.Contracts.Services;
using Shared.Events;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace PrismAndMefExample.Service
{

    public class ScannerService : IScannerService
    {
        #region Properties
        public int SelectedParity { get; set; }
        public int SelectedBitsPerSeconds { get; set; }
        public int SelectedDataBits { get; set; }
        public int SelectedStopBits { get; set; }
        public string SerialPortName { get; set; }
        #endregion

        private SerialPort _port;
        private string _dataReceived;
        private readonly IEventAggregator _eventAggregator;

        public ScannerService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SelectedBitsPerSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["BitsPerSeconds"]);
            SelectedDataBits = Convert.ToInt32(ConfigurationManager.AppSettings["DataBits"]);
            SelectedParity = Convert.ToInt32(ConfigurationManager.AppSettings["Parity"]);
            SelectedStopBits = Convert.ToInt32(ConfigurationManager.AppSettings["StopBits"]);
            SerialPortName = ConfigurationManager.AppSettings["SerialPortName"];

        }



        public void Start()
        {
            try
            {
                if (IsPortNameValid())
                {
                    _port = new SerialPort(SerialPortName)
                    {
                        BaudRate = SelectedBitsPerSeconds,
                        DataBits = SelectedDataBits,
                        Parity = (Parity)SelectedParity,
                        StopBits = (StopBits)SelectedStopBits,


                        DtrEnable = true,
                        Handshake = Handshake.None,
                        RtsEnable = true,
                        DiscardNull = false

                    };

                    _port.DataReceived -= PortOnDataReceived;
                    _port.DataReceived += PortOnDataReceived;

                    _port.Open();
                }
                else
                {
                    throw new Exception("The serial port is not configured, please select the serial port name in the Application Settings");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _dataReceived += _port.ReadExisting();

                _eventAggregator.GetEvent<ScanningEvent>()
                    .Publish(new ScanningEventArgs()
                    {
                        Data = _dataReceived.ClearData(),
                    });

                ClearBUffer();

            });
        }

        private void ClearBUffer()
        {
            _dataReceived = string.Empty;
            _port.DiscardInBuffer();

        }

        private bool IsPortNameValid()
        {
            return SerialPort.GetPortNames().Contains(SerialPortName);
        }

        public void Stop()
        {
            if (_port == null) return;
            if (_port.IsOpen) _port.Close();
            _port.DataReceived -= PortOnDataReceived;
            _port.Dispose();
        }

        public void UpdateSerialPortSettings()
        {
            Stop();

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["SerialPortName"].Value = SerialPortName;
            configuration.AppSettings.Settings["BitsPerSeconds"].Value = SelectedBitsPerSeconds.ToString();
            configuration.AppSettings.Settings["DataBits"].Value = SelectedDataBits.ToString();
            configuration.AppSettings.Settings["Parity"].Value = SelectedParity.ToString();
            configuration.AppSettings.Settings["StopBits"].Value = SelectedStopBits.ToString();

            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Start();
        }

        public IDictionary<string, int> GetParityOptions()
        {
            return new Dictionary<string, int>()
            {
                {"None",(int)Parity.None},
                {"Odd",(int)Parity.Odd},
                {"Even",(int)Parity.Even},
                {"Mark",(int)Parity.Mark },
                {"Space",(int)Parity.Space }
            };
        }

        public IDictionary<string, int> StopBitsOptions()
        {
            return new Dictionary<string, int>()
            {
                {"None",(int)StopBits.None },
                {"One", (int)StopBits.One },
                {"Two",(int)StopBits.Two },
                {"One point five",(int)StopBits.OnePointFive }
            };
        }

        public IEnumerable<int> GetBitsPerSecondOptions()
        {
            return new[] { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
        }

        public IEnumerable<int> GetDataBitsOptions()
        {
            return new[] { 5, 6, 7, 8 };
        }

        public IEnumerable<string> GetSerialPortNames()
        {
            return SerialPort.GetPortNames();
        }
    }
}
