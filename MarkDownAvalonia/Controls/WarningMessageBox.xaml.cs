using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarkDownAvalonia.Controls
{
    public class WarningMessageBox : Window
    {
        private string title;
        private string content;
        
        public WarningMessageBox(string title, string content)
        {
            AvaloniaXamlLoader.Load(this);
            this.title = title;
            this.content = content;
        }

        public WarningMessageBox():this("Message",String.Empty)
        {
        }
        
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}