using System;
using System.Threading.Tasks;
using MarkDownAvalonia.Controls;
using Avalonia.Controls;

namespace MarkDownAvalonia.Data
{
    public static class MessageBox
    {
        private static readonly double MESSAGE_BOX_WIDTH = 480;
        private static readonly double MESSAGE_BOX_HEIGHT = 300;
        
        public static Task showSuccess(Window sender, String message)
        {
            SuccessMessageBox successMessageBox = new SuccessMessageBox(null, message);
            successMessageBox.Width = MESSAGE_BOX_WIDTH;
            successMessageBox.Height = MESSAGE_BOX_HEIGHT;
            return successMessageBox.ShowDialog(sender);
        }
        
        public static Task showError(Window sender, String message)
        {
            ErrorMessageBox errorMessageBox = new ErrorMessageBox(null, message);
            errorMessageBox.Width = MESSAGE_BOX_WIDTH;
            errorMessageBox.Height = MESSAGE_BOX_HEIGHT;
            return errorMessageBox.ShowDialog(sender);
        }
        
        public static Task showInfo(Window sender, String message)
        {
            InfoMessageBox infoMessageBox = new InfoMessageBox(null, message);
            infoMessageBox.Width = MESSAGE_BOX_WIDTH;
            infoMessageBox.Height = MESSAGE_BOX_HEIGHT;
            return infoMessageBox.ShowDialog(sender);
        }
        
        public static Task<bool> showWarnning(Window sender, String message)
        {
            WarningMessageBox warningMessageBox = new WarningMessageBox(null, message);
            warningMessageBox.Width = MESSAGE_BOX_WIDTH;
            warningMessageBox.Height = MESSAGE_BOX_HEIGHT;
            return warningMessageBox.ShowDialog<bool>(sender);
        }

    }
}