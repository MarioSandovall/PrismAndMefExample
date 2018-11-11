
using Autofac;
using PrismAndMefExample.Startup;
using PrismAndMefExample.ViewModel;
using System.Windows;

namespace PrismAndMefExample
{

    public partial class App : Application
    {
        private MainViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            var container = Bootstrapper.Instance.BootStrap();
            _mainViewModel = container.Resolve<MainViewModel>();

            var mainWindow = new MainWindow { DataContext = _mainViewModel };
            mainWindow.Loaded += (sender, args) => _mainViewModel.Load();

            _mainViewModel.Theme();
            mainWindow.Show();
        }
    }
}
