using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace Shared.Contracts.Services
{
    public interface IDialogService
    {
        Task<MessageDialogResult> AskQuestionAsync(string title, string message);
        Task<ProgressDialogController> ShowProgressAsync(string message = "");
        Task ShowMessageAsync(string title, string message);
    }
}