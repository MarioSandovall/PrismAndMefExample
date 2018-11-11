using Shared.Contracts.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Application_1.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>

    [Export(typeof(ISettingsUserControl))]
    public partial class SettingsViews : UserControl, ISettingsUserControl
    {
        public SettingsViews()
        {
            InitializeComponent();
        }
    }
}
