using Prism.Commands;
using Prism.Services.Dialogs;
using SpireHL.Core.Models;

namespace SpireHL.Base.Dialogs
{
    public class ConfirmationViewModel : DialogViewModelBase
    {
        public DelegateCommand<string> CloseDialogCommand { get; }

        public ConfirmationViewModel()
        {
            Title = "Confirmation Dialog";
            CloseDialogCommand = new DelegateCommand<string>(CloseDialog);
        }

        private void CloseDialog(string parameter)
        {
            if (parameter?.ToLower() == "true")
                RaiseRequestClose(new DialogResult(ButtonResult.Yes));
            else
                RaiseRequestClose(new DialogResult(ButtonResult.No));
        }
    }
}
