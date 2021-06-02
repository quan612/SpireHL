using CatalogModule.Models;
using Prism.Commands;
using Prism.Services.Dialogs;
using SpireHL.Core.Models;

namespace CatalogModule.Dialogs
{
    class UserOptionsCatalogDialogViewModel : DialogViewModelBase
    {
        public DelegateCommand CloseDialogCommand { get; }

        public UserOptionsCatalogDialogViewModel()
        {
            Title = "User Select Options Dialog";
            CloseDialogCommand = new DelegateCommand(CloseDialog);
            UserSelect = new UserSelectOptions();
        }

        private void CloseDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("UserSelect", UserSelect);
            RaiseRequestClose(new DialogResult(ButtonResult.OK, parameters));
        }

        private UserSelectOptions _userSelect;

        public UserSelectOptions UserSelect
        {
            get { return _userSelect; }
            set { _userSelect = value; }
        }


        public override void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
