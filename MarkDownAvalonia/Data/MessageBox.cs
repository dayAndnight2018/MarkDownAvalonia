using System;
using System.Threading.Tasks;
using MarkDownAvalonia.Controls;
using Avalonia.Controls;

namespace MarkDownAvalonia.Data
{
    public static class MessageBox
    {
        public static Task showSuccess(Window sender, String message)
        {
            SuccessMessageBox successMessageBox = new SuccessMessageBox(null, message);
            successMessageBox.Width = 480;
            successMessageBox.Height = 300;
            return successMessageBox.ShowDialog(sender);
        }
        
        public static Task showError(Window sender, String message)
        {
            ErrorMessageBox errorMessageBox = new ErrorMessageBox(null, message);
            errorMessageBox.Width = 480;
            errorMessageBox.Height = 300;
            return errorMessageBox.ShowDialog(sender);
        }
        
        public static Task showInfo(Window sender, String message)
        {
            InfoMessageBox infoMessageBox = new InfoMessageBox(null, message);
            infoMessageBox.Width = 480;
            infoMessageBox.Height = 300;
            return infoMessageBox.ShowDialog(sender);
        }
        
        public static Task showWarnning(Window sender, String message)
        {
            WarningMessageBox warningMessageBox = new WarningMessageBox(null, message);
            warningMessageBox.Width = 480;
            warningMessageBox.Height = 300;
            return warningMessageBox.ShowDialog(sender);
        }

    }
}