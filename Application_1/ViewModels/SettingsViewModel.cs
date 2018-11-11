using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Shared.Contracts.Services;
using Shared.Contracts.ViewModel;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;

namespace Application_1.ViewModels
{
    [Export(typeof(ISettingsViewModel))]
    public class SettingsViewModel : BindableBase, ISettingsViewModel
    {

        #region Properties

        private string _selectedSerialPortName;

        public string SelectedSerialPortName
        {
            get => _selectedSerialPortName;
            set => SetProperty(ref _selectedSerialPortName, value);
        }

        private int _selectedParity;
        public int SelectedParity
        {
            get => _selectedParity;
            set => SetProperty(ref _selectedParity, value);
        }
        private int _selectedBitsPerSeconds;
        public int SelectedBitsPerSeconds
        {
            get => _selectedBitsPerSeconds;
            set => SetProperty(ref _selectedBitsPerSeconds, value);
        }

        private int _selectedDataBits;
        public int SelectedDataBits
        {
            get => _selectedDataBits;
            set => SetProperty(ref _selectedDataBits, value);
        }

        private int _selectedStopBits;
        public int SelectedStopBits
        {
            get => _selectedStopBits;
            set => SetProperty(ref _selectedStopBits, value);
        }

        private string _serialPortName;
        public string SerialPortName
        {
            get => _serialPortName;
            set => SetProperty(ref _serialPortName, value);
        }

        public IList<string> SerialPortNames { get; set; }
        public IList<int> BitsPerSeconds { get; set; }
        public IList<int> DataBits { get; set; }
        public IDictionary<string, int> Parity { get; set; }
        public IDictionary<string, int> StopBits { get; set; }


        #endregion

        #region Commands

        public ICommand CloseCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        #endregion

        private readonly IScannerService _scannerService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;

        [ImportingConstructor]
        public SettingsViewModel(
          IEventAggregator eventAggregator,
          IScannerService scannerService,
          IDialogService dialogService)
        {

            _scannerService = scannerService;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            SerialPortNames = new List<string>();
            BitsPerSeconds = new List<int>();
            DataBits = new List<int>();
            Parity = new Dictionary<string, int>();
            StopBits = new Dictionary<string, int>();


            CloseCommand = new DelegateCommand(OnCloseExecute);
            SaveCommand = new DelegateCommand(OnSaveExecute);

            InitializeSettingsValues();
        }

        public async void Load()
        {
            try
            {
                _scannerService.Start();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;

                await _dialogService.ShowMessageAsync("Settings Error", ex.Message);
            }
        }

        private void InitializeSettingsValues()
        {
            _scannerService.GetSerialPortNames().ToList().ForEach(x => SerialPortNames.Add(x));
            _scannerService.GetBitsPerSecondOptions().ToList().ForEach(x => BitsPerSeconds.Add(x));
            _scannerService.GetParityOptions().ToList().ForEach(x => Parity.Add(x));
            _scannerService.GetDataBitsOptions().ToList().ForEach(x => DataBits.Add(x));
            _scannerService.StopBitsOptions().ToList().ForEach(x => StopBits.Add(x));

            SelectedSerialPortName = _scannerService.SerialPortName;
            SelectedBitsPerSeconds = _scannerService.SelectedBitsPerSeconds;
            SelectedParity = _scannerService.SelectedParity;
            SelectedDataBits = _scannerService.SelectedDataBits;
            SelectedStopBits = _scannerService.SelectedStopBits;
        }

        private async void OnSaveExecute()
        {
            try
            {
                _scannerService.SerialPortName = SelectedSerialPortName;
                _scannerService.SelectedBitsPerSeconds = SelectedBitsPerSeconds;
                _scannerService.SelectedParity = SelectedParity;
                _scannerService.SelectedDataBits = SelectedDataBits;
                _scannerService.SelectedStopBits = SelectedStopBits;

                _scannerService.UpdateSerialPortSettings();
                OnCloseExecute();

                await _dialogService.ShowMessageAsync("Serial Port", "Configuration completed successfully");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;

                await _dialogService.ShowMessageAsync("Settings Error", ex.Message);
            }

        }

        private void OnCloseExecute()
        {
            _eventAggregator.GetEvent<SettingsEvent>().Publish(false);
        }


    }
}
