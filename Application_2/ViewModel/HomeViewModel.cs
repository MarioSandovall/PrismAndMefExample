using Shared.Contracts.ViewModel;
using System.ComponentModel.Composition;

namespace Application_2.ViewModel
{
    [Export(typeof(IHomeViewModel))]
    public class HomeViewModel: IHomeViewModel
    {
        public HomeViewModel()
        {
            
        }
    }
}
