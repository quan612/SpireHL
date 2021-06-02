using Prism.Commands;
using Prism.Services.Dialogs;
using SpireHL.Core.Models;

namespace SpireHL.Dialogs
{
    public class ExceptionDialogViewModel : DialogViewModelBase
    {
        public DelegateCommand CloseDialogCommand { get; }

        public ExceptionDialogViewModel()
        {
            Title = "Exception";
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private void CloseDialog()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        private string _dialogStackTrace = string.Empty;
        public string DialogStackTrace
        {
            get { return _dialogStackTrace; }
            set { SetProperty(ref _dialogStackTrace, value); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("message"))
            {
                DialogMessage = parameters.GetValue<string>("message");
            }

            if (parameters.ContainsKey("stackTrace"))
            {
                DialogStackTrace = parameters.GetValue<string>("stackTrace");
            }
        }
    }
}
