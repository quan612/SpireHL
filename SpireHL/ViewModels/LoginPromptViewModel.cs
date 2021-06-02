using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace IkanManage.ViewModels
{
    public class LoginPromptViewModel : BindableBase, IDialogAware
    {
        private readonly IRegionManager _regionManager;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            return;
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            return;
        }

        public string Title => "ARx Common Login";
        
        public event Action<IDialogResult> RequestClose;

        public DelegateCommand CloseLoginPromptCommand { get; }

        public LoginPromptViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CloseLoginPromptCommand = new DelegateCommand(CloseLoginWindow);
        }

        private void CloseLoginWindow()
        {
            var loginResult = ButtonResult.Cancel;
            var parms = new DialogParameters();
            parms.Add("CloseCommand", "Closed by the User");

            RequestClose?.Invoke(new DialogResult(loginResult, parms));
        }

    }
}
