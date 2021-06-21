using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Markdown.Avalonia;

namespace MarkDownAvalonia.Controls
{
    public class EditorWindow : Window
    {
        public EditorWindow()
        {
            InitializeComponent();
            this.FindControl<TextBox>("inputTextBox").Text =
                $"---\r\n\r\ntitle: \r\n\r\ndate: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\r\n\r\ntags: \r\n\r\n---\r\n\r\n";
            this.FindControl<MarkdownScrollViewer>("markdownPreview").Markdown = 
                $"---\r\n\r\ntitle: \r\n\r\ndate: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\r\n\r\ntags: \r\n\r\n---\r\n\r\n";
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public void OnButtonClick(object sender, RoutedEventArgs e)
        {
            this.FindControl<Button>("btn").Content = "test";
            new OpenFileDialog().ShowAsync(this);
        }

        public void OpenMenuClicked(object sender, RoutedEventArgs e)
        {
            new OpenFolderDialog().ShowAsync(this);
        }

        public void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        public void LabelMouseEntered(object sender, PointerEventArgs e)
        {
            var button = sender as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(199,80,73));
            button.FontWeight = FontWeight.Bold;
        }
        
        public void LabelMouseLeave(object sender, PointerEventArgs e)
        {
            var button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.FontWeight = FontWeight.Normal;
        }

        public void TbxKeyUp(object sender, KeyEventArgs e)
        {
            var tbx = sender as TextBox;
            this.FindControl<MarkdownScrollViewer>("markdownPreview").Markdown = tbx.Text;
        }

    }
}