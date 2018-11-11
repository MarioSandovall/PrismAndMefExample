using Shared.Contracts.Views;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Application_1.Views
{
    [Export(typeof(IHomeUserControl))]

    public partial class HomeView : UserControl, IHomeUserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}
