using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Markdown.Avalonia;
using MarkDownAvalonia.Controls;
using MarkDownAvalonia.Data;
using Color = Avalonia.Media.Color;

namespace MarkDownAvalonia
{
    public class MainWindow : Window
    {
        private TextBox inputTbx;
        private StackPanel articleListPanel;
        private MarkdownScrollViewer markdownPreview;
        public MainWindow()
        {
            InitializeComponent();
            this.articleListPanel = this.FindControl<StackPanel>("postItemsPanel");
            this.inputTbx = this.FindControl<TextBox>("inputTextBox");
            this.markdownPreview = this.FindControl<MarkdownScrollViewer>("markdownPreview");
            
            IEnumerable<string> files = Directory.GetFiles(CommonData.config.RootDirectory).Where(f=>f.EndsWith(".md"));
            foreach (var file in files)
            {
                PostItemControl current = new PostItemControl(file, this.inputTbx);
                current.Register(new EventHandler<PointerPressedEventArgs>((sender, e) =>
                {
                    foreach (PostItemControl control in articleListPanel.Children)
                    {
                        control.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
                        control.Foreground = new SolidColorBrush(Colors.Silver);
                        control.Remove();
                    }
                    current.Background = new SolidColorBrush(Color.FromRgb(199,80,73));
                    current.Foreground = new SolidColorBrush(Colors.White);
                    current.configTimer();
                }));
                articleListPanel.Children.Add(current);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            // InfoMessageBox messageBox = new InfoMessageBox();
            // messageBox.Width = 480;
            // messageBox.Height = 300;
            // messageBox.ShowDialog(this);
            //
            // SuccessMessageBox success = new SuccessMessageBox();
            // success.Width = 480;
            // success.Height = 300;
            // success.ShowDialog(this);
            //
            // WarningMessageBox warning = new WarningMessageBox();
            // warning.Width = 480;
            // warning.Height = 300;
            // warning.ShowDialog(this);
            //
            // ErrorMessageBox errorMessageBox = new ErrorMessageBox();
            // errorMessageBox.Width = 480;
            // errorMessageBox.Height = 300;
            // errorMessageBox.ShowDialog(this);
        }

        public void NewPost(object sender, RoutedEventArgs e)
        {
            PostItemControl has = findTempFile();
            if (has != null)
            {
                return;
            }
            // EditorWindow editorWindow = new EditorWindow();
            // editorWindow.ShowDialog(this);
            PostItemControl current = new PostItemControl(this.inputTbx);
            current.Register(new EventHandler<PointerPressedEventArgs>((sender, e) =>
            {
                foreach (PostItemControl control in articleListPanel.Children)
                {
                    control.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
                    control.Foreground = new SolidColorBrush(Colors.Silver);
                    control.Remove();
                }
                current.Background = new SolidColorBrush(Color.FromRgb(199,80,73));
                current.Foreground = new SolidColorBrush(Colors.White);
                current.configTimer();
            }));
            inputTbx.Text = $"---\r\n\r\ntitle: \r\n\r\ndate: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\r\n\r\ntags: \r\n\r\n---\r\n\r\n";
            markdownPreview.Markdown = $"---\r\n\r\ntitle: \r\n\r\ndate: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\r\n\r\ntags: \r\n\r\n---\r\n\r\n";
            current.updateCache();
            articleListPanel.Children.Insert(0, current);
        }

        public void OpenMenuClicked(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.ShowAsync(this);
        }

        /// <summary>
        /// exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// open setting window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OpenSettingWindow(Object sender, RoutedEventArgs e)
        {
            SettingWindow mb = new SettingWindow();
            mb.Width = 500;
            mb.Height = 320;
            mb.ShowDialog(this);
        }
        
        /// <summary>
        /// button effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LabelMouseEntered(object sender, PointerEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(199,80,73));
            button.FontWeight = FontWeight.Bold;
        }
        
        /// <summary>
        /// button effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LabelMouseLeave(object sender, PointerEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.FontWeight = FontWeight.Normal;
        }

        /// <summary>
        /// flush preview when input text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TextChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name.Equals("Text"))
            {
                markdownPreview.Markdown = this.inputTbx.Text;
            }
        }
        
        private PostItemControl findTempFile()
        {
            foreach (PostItemControl control in this.articleListPanel.Children)
            {
                if (!control.isExists)
                {
                    return control;
                }
            }

            return null;
        }
    }
}