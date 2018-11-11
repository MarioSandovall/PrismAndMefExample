using Autofac;
using PrismAndMefExample.Startup;
using Shared.Contracts.Views;
using System.Windows.Controls;

namespace PrismAndMefExample.Views
{

    public partial class SettingsView : UserControl
    {
        //[Import(typeof(ISettingsUserControl))]
        //private ISettingsUserControl _settingsUserControl;

        public SettingsView()
        {
            var container = Bootstrapper.Instance.Cointainer;
            var userControl = container.Resolve<ISettingsUserControl>();

            InitializeComponent();

            SettingsUserControl.Children.Clear();
            SettingsUserControl.Children.Add(userControl as UserControl);
        }
    }
}
