using Prism.Commands;
using Prism.Services.Dialogs;
using SpireHL.Core.Models;
using System;
using System.Windows.Input;

namespace SpireHL.Dialogs
{
    class SummaryDialogViewModel : DialogViewModelBase
    {
        public DelegateCommand CloseDialogCommand { get; }

        public SummaryDialogViewModel()
        {
            Title = "Summary Dialog";
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        private void CloseDialog()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            PathToFile = parameters.GetValue<string>(nameof(PathToFile));
        }

        #region View File
        private string _pathToFile;
        public string PathToFile
        {
            get { return _pathToFile; }
            set { SetProperty(ref _pathToFile, value); }
        }

        private DelegateCommand _viewFileSaved;

        public ICommand ViewFileSaved => _viewFileSaved ?? (_viewFileSaved = new DelegateCommand(ViewFile));

        private void ViewFile()
        {
            if (!String.IsNullOrEmpty(PathToFile))
            {
                System.Diagnostics.Process.Start(PathToFile);
            }
        }
        #endregion
    }
}
