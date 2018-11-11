using Application_2.Models;
using MahApps.Metro;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Shared.Contracts.Services;
using Shared.Contracts.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Shared.Events;

namespace Application_2.ViewModel
{
    [Export(typeof(ISettingsViewModel))]
    public class SettingsViewModel : BindableBase, ISettingsViewModel
    {
        #region Properties

        public ObservableCollection<AccentColor> AccentColors { get; set; }

        public ObservableCollection<AccentColor> Themes { get; set; }

        private AccentColor _selectedAccentColor;
        public AccentColor SelectedAccentColor
        {
            get => _selectedAccentColor;
            set => SetProperty(ref _selectedAccentColor, value);
        }

        private AccentColor _selectedTheme;
        public AccentColor SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        #endregion

        #region Commands

        public ICommand CloseCommand { get; set; }

        public ICommand SaveCommand { get; set; }


        #endregion

        private readonly IDialogService _dialogService;
        private readonly IWindowFactory _windowFactory;
        private readonly IEventAggregator _eventAggregator;
        private readonly IConfigurationService _configurationService;

        [ImportingConstructor]
        public SettingsViewModel(
            IEventAggregator eventAggregator,
            IConfigurationService configurationService,
            IDialogService dialogService,
            IWindowFactory windowFactory)
        {

            _windowFactory = windowFactory;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _configurationService = configurationService;

            AccentColors = new ObservableCollection<AccentColor>();
            Themes = new ObservableCollection<AccentColor>();

            SaveCommand = new DelegateCommand(OnSaveExecute);
            CloseCommand = new DelegateCommand(OnCloseExecute);

        }
        public async void Load()
        {
            try
            {

                InitializeThemes();
                InitializeAccentColors();

                var window = _windowFactory.GetMainWindow();
                if (window == null) return;

                var appStyle = ThemeManager.DetectAppStyle(window);
                var theme = appStyle.Item1.Name;
                var color = appStyle.Item2.Name;

                SelectedTheme = Themes.Single(x => x.Name == theme);
                SelectedAccentColor = AccentColors.Single(x => x.Name == color);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                await _dialogService.ShowMessageAsync("Internal error", ex.Message);
            }

        }

        public async void OnSaveExecute()
        {

            try
            {
                _configurationService.UpdateTheme(SelectedTheme.Name, SelectedAccentColor.Name);
                await _dialogService.ShowMessageAsync("Theme", "The interface has been updated correctly");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                await _dialogService.ShowMessageAsync("Internal error", ex.Message);
            }
        }

        private void OnCloseExecute()
        {
            _eventAggregator.GetEvent<SettingsEvent>().Publish(false);
        }

        private void InitializeAccentColors()
        {
            AccentColors.Clear();
            foreach (var accent in ThemeManager.Accents)
            {
                var color = new AccentColor()
                {
                    Name = accent.Name,
                    Color = accent.Resources["AccentColor"].ToString()
                };

                AccentColors.Add(color);
            }
        }

        private void InitializeThemes()
        {
            Themes.Clear();
            foreach (var theme in ThemeManager.AppThemes)
            {
                var color = new AccentColor()
                {
                    Name = theme.Name,
                    Color = theme.Resources["WhiteColorBrush"].ToString()
                };

                Themes.Add(color);
            }

        }
    }
}
