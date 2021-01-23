using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace MarkDownAvalonia.Controls
{
    public class FindWindow : Window
    {
        private TextBox input;
        private TextBox replace;
        
        private TextBox mainWindowTextBox;
        private int matchOrder = -1;
        private String cacheMatchText = null;
        private int lastIndex = -1;

        public FindWindow()
        {
            AvaloniaXamlLoader.Load(this);
            input = this.FindControl<TextBox>("inputBox");
            replace = this.FindControl<TextBox>("replaceBox");
            mainWindowTextBox.SelectionBrush = new SolidColorBrush(Colors.Black);
            mainWindowTextBox.SelectionForegroundBrush = new SolidColorBrush(Colors.White);
        }

        public FindWindow(TextBox textBox)
        {
            AvaloniaXamlLoader.Load(this);
            mainWindowTextBox = textBox;
            input = this.FindControl<TextBox>("inputBox");
            replace = this.FindControl<TextBox>("replaceBox");
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        public void NextMatch(object sender, RoutedEventArgs e)
        {
            String text = mainWindowTextBox.Text;
            String searchText = input.Text;
            if (!searchText.Equals(cacheMatchText))
            {
                // search text changed 
                matchOrder = -1;
                lastIndex = -1;
                cacheMatchText = input.Text;
            }

            int currentMatchIndex = -1;
            if ((currentMatchIndex = text.IndexOf(searchText, lastIndex + 1)) != -1)
            {
                mainWindowTextBox.SelectionBrush = new SolidColorBrush(Colors.Black);
                mainWindowTextBox.SelectionForegroundBrush = new SolidColorBrush(Colors.White);
                mainWindowTextBox.SelectionStart = currentMatchIndex;
                mainWindowTextBox.SelectionEnd = currentMatchIndex + searchText.Length;
                // update new matched
                matchOrder++;
                lastIndex = currentMatchIndex;
            }
        }

        public void LastMatch(object sender, RoutedEventArgs e)
        {
            String text = mainWindowTextBox.Text;
            String searchText = input.Text;
            if (!searchText.Equals(cacheMatchText))
            {
                // search text changed 
                matchOrder = -1;
                lastIndex = text.Length;
                cacheMatchText = input.Text;
            }

            int currentMatchIndex = -1;
            if ((currentMatchIndex = text.Substring(0, lastIndex - 1 < 0 ? 0 : lastIndex - 1).LastIndexOf(searchText)) != -1)
            {
                mainWindowTextBox.SelectionBrush = new SolidColorBrush(Colors.Black);
                mainWindowTextBox.SelectionForegroundBrush = new SolidColorBrush(Colors.White);
                mainWindowTextBox.SelectionStart = currentMatchIndex;
                mainWindowTextBox.SelectionEnd = currentMatchIndex + searchText.Length;
                // update new matched
                matchOrder++;
                lastIndex = currentMatchIndex;
            }
        }
        
        public void Replace(object sender, RoutedEventArgs e)
        {
            String text = mainWindowTextBox.Text;
            String searchText = input.Text;
            if (searchText.Equals(mainWindowTextBox.SelectedText))
            {
                mainWindowTextBox.SelectedText = replace.Text;
            }
        }
        
        public void ReplaceAll(object sender, RoutedEventArgs e)
        {
            String text = mainWindowTextBox.Text;
            String searchText = input.Text;
            mainWindowTextBox.Text = mainWindowTextBox.Text.Replace(searchText, replace.Text);
        }
    }
}