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
        private readonly Label titleLabel;
        private readonly TextBox contentTbx;
        
        public NewPostWindow(string title, string content)
        {
            AvaloniaXamlLoader.Load(this);
            this.title = title;
            this.content = content;
            
            titleLabel = this.FindControl<Label>("title");
            contentTbx = this.FindControl<TextBox>("content");

            titleLabel.Content = title;
            contentTbx.Text = content;
            contentTbx.Width = Width / 5.0 * 4;
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