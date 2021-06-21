using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace MarkDownAvalonia.Controls
{
    public class FindWindow : Window
    {
        private readonly TextBox input;
        private readonly TextBox replace;
        private readonly TextBox mainWindowTextBox;
        private readonly Border border;

        private int matchOrder = -1;
        private string cacheMatchText;
        private int lastIndex = -1;

        public FindWindow(TextBox textBox)
        {
            AvaloniaXamlLoader.Load(this);
            mainWindowTextBox = textBox;
            input = this.FindControl<TextBox>("inputBox");
            replace = this.FindControl<TextBox>("replaceBox");
            border = this.FindControl<Border>("windowBorder");

            border.BorderBrush = new LinearGradientBrush()
            {
                GradientStops = new GradientStops()
                {
                    new GradientStop()
                    {
                        Color = Colors.Silver,
                        Offset = 0
                    },
                    new GradientStop()
                    {
                        Color = Color.Parse("#f2f2f2"),
                        Offset = 0
                    }
                }
            };
        }

        public FindWindow()
        {
            AvaloniaXamlLoader.Load(this);
            input = this.FindControl<TextBox>("inputBox");
            replace = this.FindControl<TextBox>("replaceBox");
            border = this.FindControl<Border>("windowBorder");
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        /// <summary>
        /// 下一个匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NextMatch(object sender, RoutedEventArgs e)
        {
            var text = mainWindowTextBox.Text;
            var searchText = input.Text;

            // 异常输入
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            // 切换搜索文本
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
                mainWindowTextBox.SelectionBrush = new SolidColorBrush(Colors.DodgerBlue);
                mainWindowTextBox.SelectionForegroundBrush = new SolidColorBrush(Colors.White);
                mainWindowTextBox.SelectionStart = currentMatchIndex;
                mainWindowTextBox.SelectionEnd = currentMatchIndex + searchText.Length;
                // update new matched
                matchOrder++;
                lastIndex = currentMatchIndex;
            }
        }

        /// <summary>
        /// 上一个匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LastMatch(object sender, RoutedEventArgs e)
        {
            var text = mainWindowTextBox.Text;
            var searchText = input.Text;

            // error input
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            // new search
            if (!searchText.Equals(cacheMatchText))
            {
                // search text changed 
                matchOrder = -1;
                lastIndex = text.Length;
                cacheMatchText = input.Text;
            }

            var currentMatchIndex = -1;
            var searchEndIndex = Math.Max(lastIndex, 0);
            if ((currentMatchIndex =
                text.Substring(0, searchEndIndex).LastIndexOf(searchText)) != -1)
            {
                mainWindowTextBox.SelectionBrush = new SolidColorBrush(Colors.DodgerBlue);
                mainWindowTextBox.SelectionForegroundBrush = new SolidColorBrush(Colors.White);
                mainWindowTextBox.SelectionStart = currentMatchIndex;
                mainWindowTextBox.SelectionEnd = currentMatchIndex + searchText.Length;
                // update new matched
                matchOrder++;
                lastIndex = currentMatchIndex;
            }
        }

        /// <summary>
        /// replace text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Replace(object sender, RoutedEventArgs e)
        {
            var text = mainWindowTextBox.Text;
            var searchText = input.Text;

            // 异常输入
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            if (searchText.Equals(mainWindowTextBox.SelectedText))
            {
                mainWindowTextBox.SelectedText = replace.Text;
            }
        }

        /// <summary>
        /// replace all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReplaceAll(object sender, RoutedEventArgs e)
        {
            var text = mainWindowTextBox.Text;
            var searchText = input.Text;

            // 异常输入
            if (string.IsNullOrWhiteSpace(searchText))
                return;

            mainWindowTextBox.Text = mainWindowTextBox.Text.Replace(searchText, replace.Text);
        }
    }
}