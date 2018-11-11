using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PrismAndMefExample.Startup;
using Shared.Contracts.Services;
using System.Threading.Tasks;

namespace PrismAndMefExample.Service
{

    public class DialogService : IDialogService
    {

        private MetroWindow _dialog;
        public DialogService(IWindowFactory windowFactory)
        {
            _dialog = windowFactory.GetMainWindow();
        }

        public Task<MessageDialogResult> AskQuestionAsync(string title, string message)
        {
            if (_dialog == null) ValidateWindow();
            return _dialog.ShowMessageAsync(title, message,
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Ok",
                    NegativeButtonText = "Cancel"
                });
        }

        public Task<ProgressDialogController> ShowProgressAsync(string message = "")
        {
            if (_dialog == null) ValidateWindow();
            return _dialog.ShowProgressAsync("Working On It ... ", message);
        }


        public Task ShowMessageAsync(string title, string message)
        {
            if (_dialog == null) ValidateWindow();
            return _dialog.ShowMessageAsync(title, message);
        }

        private void ValidateWindow()
        {
            var container = Bootstrapper.Instance.Cointainer;
            var window = container.Resolve<IWindowFactory>();
            _dialog = window.GetMainWindow();
        }

    }
}
