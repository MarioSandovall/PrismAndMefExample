using Autofac;
using PrismAndMefExample.Startup;
using Shared.Contracts.Views;
using System.Windows.Controls;

namespace PrismAndMefExample.Views
{

    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            var container = Bootstrapper.Instance.Cointainer;
            var userControl = container.Resolve<IHomeUserControl>();

            InitializeComponent();

            HomeUserControl.Children.Clear();
            HomeUserControl.Children.Add(userControl as UserControl);
        }
    }
}
