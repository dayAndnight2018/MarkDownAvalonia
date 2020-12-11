using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarkDownAvalonia.Controls
{
    public class NewPostWindow : Window
    {
        private string title;
        private string content;
        
        public NewPostWindow(string title, string content)
        {
            AvaloniaXamlLoader.Load(this);
            this.title = title;
            this.content = content;
            this.FindControl<Label>("title").Content = title;
            this.FindControl<TextBox>("content").Text = content;
            this.FindControl<TextBox>("content").Width = Width / 5.0 * 4;
        }

        public NewPostWindow():this("New Post",String.Empty)
        {
        }
        
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}