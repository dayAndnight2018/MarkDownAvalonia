using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarkDownAvalonia.Controls
{
    public class ErrorMessageBox : Window
    {
        private string title;
        private string content;
        private Label messageLabel;
        
        public ErrorMessageBox(string title, string content)
        {
            AvaloniaXamlLoader.Load(this);
            this.title = title;
            this.content = content;
            messageLabel = this.FindControl<Label>("message");
            messageLabel.Content = content;
        }

        public ErrorMessageBox():this("Message",String.Empty)
        {
        }
        
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}