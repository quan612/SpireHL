//using Microsoft.Win32;
using Prism.Services.Dialogs;
using System;
using System.Windows.Forms;

namespace SpireHL.Core.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowException(this IDialogService dialogService, Exception ex)
        {
            var parameters = new DialogParameters();
            parameters.Add("message", ex.Message);
            parameters.Add("stackTrace", ex.StackTrace);
            dialogService.ShowDialog("ExceptionDialog", parameters, callback => { });
        }

        public static void ShowUserSelectOption(this IDialogService dialogService, Action<IDialogResult> callback)
        {
            dialogService.ShowDialog("UserSelectOptionsDialog", new DialogParameters(), callback);
        }

        public static void ShowSummary(this IDialogService dialogService, DialogParameters parameters)
        {
            dialogService.ShowDialog("SummaryDialog", parameters, callback => { });
        }

        public static void ShowConfirmation(this IDialogService dialogService, string confirmation, Action<IDialogResult> callback)
        {
            dialogService.ShowDialog("Confirmation", new DialogParameters($"message={confirmation}"), callback);
        }

        public static string ShowOpenFileDialog(this IDialogService dialogService,
            string defaultDirectory = "C:\\",
            string filter = "Excel documents (*.xls, *.xlsx)|*.xls;*.xlsx",
            string defaultExt = ".xls|.xlsx")
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = defaultExt;
            dlg.Filter = filter;
            dlg.InitialDirectory = defaultDirectory;

            var result = dlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                return dlg.FileName;
            }
            return string.Empty;
        }

        public static string ShowSetPathDialog(this IDialogService dialogService, string defaultDirectory = "C:\\")
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            var result = dlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                return dlg.SelectedPath;
            }
            return string.Empty;
        }
    }
}
