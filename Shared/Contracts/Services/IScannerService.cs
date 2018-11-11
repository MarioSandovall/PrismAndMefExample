using System.Collections.Generic;

namespace Shared.Contracts.Services
{
    public interface IScannerService
    {
        void Start();
        void Stop();

        int SelectedParity { get; set; }
        int SelectedBitsPerSeconds { get; set; }
        int SelectedDataBits { get; set; }
        int SelectedStopBits { get; set; }
        string SerialPortName { get; set; }
        void UpdateSerialPortSettings();
        IEnumerable<string> GetSerialPortNames();
        IDictionary<string, int> GetParityOptions();
        IDictionary<string, int> StopBitsOptions();
        IEnumerable<int> GetBitsPerSecondOptions();
        IEnumerable<int> GetDataBitsOptions();
    }
}
