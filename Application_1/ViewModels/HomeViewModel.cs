using Shared.Contracts.ViewModel;
using System.ComponentModel.Composition;

namespace Application_1.ViewModels
{
    [Export(typeof(IHomeViewModel))]

    public class HomeViewModel: IHomeViewModel
    {

    }
}
