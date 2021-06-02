using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace SpireHL.Core.Models
{
    public class DialogViewModelBase : BindableBase, IDialogAware
    {
        public bool CanCloseDialog() => true;

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _dialogMessage = string.Empty;
        public string DialogMessage
        {
            get { return _dialogMessage; }
            set { SetProperty(ref _dialogMessage, value); }
        }

        public event Action<IDialogResult> RequestClose;
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("message"))
                DialogMessage = parameters.GetValue<string>("message");
        }
    }
}
