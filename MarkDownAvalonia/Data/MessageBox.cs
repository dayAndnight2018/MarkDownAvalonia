using System;
using System.Threading.Tasks;
using MarkDownAvalonia.Controls;
using Avalonia.Controls;

namespace MarkDownAvalonia.Data
{
    /**
     * message box helper
     */
    public static class MessageBox
    {
        /// <summary>
        /// width of message box
        /// </summary>
        private static readonly double MESSAGE_BOX_WIDTH = 480;
        
        /// <summary>
        /// height of message box
        /// </summary>
        private static readonly double MESSAGE_BOX_HEIGHT = 300;
        
        /// <summary>
        /// show success message box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task ShowSuccess(Window sender, string message)
        {
            var successMessageBox = new SuccessMessageBox(null, message)
            {
                Width = MESSAGE_BOX_WIDTH,
                Height = MESSAGE_BOX_HEIGHT
            };
            return successMessageBox.ShowDialog(sender);
        }
        
        /// <summary>
        /// show error message box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task ShowError(Window sender, string message)
        {
            var errorMessageBox = new ErrorMessageBox(null, message)
            {
                Width = MESSAGE_BOX_WIDTH,
                Height = MESSAGE_BOX_HEIGHT
            };
            return errorMessageBox.ShowDialog(sender);
        }
        
        /// <summary>
        /// show info message box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task ShowInfo(Window sender, string message)
        {
            var infoMessageBox = new InfoMessageBox(null, message)
            {
                Width = MESSAGE_BOX_WIDTH,
                Height = MESSAGE_BOX_HEIGHT
            };
            return infoMessageBox.ShowDialog(sender);
        }
        
        /// <summary>
        /// show warning message box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task<bool> ShowWarning(Window sender, string message)
        {
            var warningMessageBox = new WarningMessageBox(null, message)
            {
                Width = MESSAGE_BOX_WIDTH,
                Height = MESSAGE_BOX_HEIGHT
            };
            return warningMessageBox.ShowDialog<bool>(sender);
        }

    }
}