using Shared.Contracts.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Application_2.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [Export(typeof(ISettingsUserControl))]

    public partial class SettingsView : UserControl, ISettingsUserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
