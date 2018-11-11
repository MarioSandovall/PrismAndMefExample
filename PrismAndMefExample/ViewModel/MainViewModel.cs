using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Shared.Contracts.Services;
using Shared.Contracts.ViewModel;
using Shared.Events;
using System;
using System.Windows.Input;

namespace PrismAndMefExample.ViewModel
{
    public class MainViewModel : BindableBase
    {

        #region Properties

        private bool _isSettingsOpened;

        public bool IsSettingsOpened
        {
            get => _isSettingsOpened;
            set => SetProperty(ref _isSettingsOpened, value);
        }

        public ISettingsViewModel SettingsViewModel { get; }

        public IHomeViewModel HomeViewModel { get;  }

        #endregion

        #region Commands

        public ICommand OpenSettingsCommand { get; set; }

        #endregion

        private readonly IDialogService _dialogService;
        private readonly IConfigurationService _configurationService;
        public MainViewModel(        
            IHomeViewModel homeViewModel,
            ISettingsViewModel settingsViewModel,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IConfigurationService configurationService)
        {
            _dialogService = dialogService;
            _configurationService = configurationService;

            HomeViewModel = homeViewModel;
            SettingsViewModel = settingsViewModel;

            OpenSettingsCommand = new DelegateCommand(() => OnSettings(true));

            eventAggregator.GetEvent<SettingsEvent>().Subscribe(OnSettings);
            eventAggregator.GetEvent<ScanningEvent>().Subscribe(OnScanning);
        }



        public async void Theme()
        {
            try
            {
                _configurationService.LoadTheme();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                await _dialogService.ShowMessageAsync("Internal error", ex.Message);
            }
        }

        private void OnSettings(bool isOpend)
        {
            IsSettingsOpened = isOpend;
        }

        private void OnScanning(ScanningEventArgs args)
        {
            _dialogService.ShowMessageAsync("Scanner", $"Data : {args.Data}");
        }

        public void Load()
        {
            SettingsViewModel.Load();
        }

    }
}
