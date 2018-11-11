using MahApps.Metro.Controls;
using Shared.Contracts.Services;
using System.Windows;

namespace PrismAndMefExample.Service
{

    public class WindowFactory : IWindowFactory
    {
        public MetroWindow GetMainWindow()
        {
            return (MetroWindow)Application.Current.MainWindow;
        }
    }
}

