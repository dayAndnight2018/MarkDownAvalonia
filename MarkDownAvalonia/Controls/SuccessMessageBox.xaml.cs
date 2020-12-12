using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarkDownAvalonia.Controls
{
    public class SuccessMessageBox : Window
    {
        private string title;
        private string content;
        
        public SuccessMessageBox(string title, string content)
        {
            AvaloniaXamlLoader.Load(this);
            this.title = title;
            this.content = content;
        }

        public SuccessMessageBox():this("Message",String.Empty)
        {
        }
        
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}