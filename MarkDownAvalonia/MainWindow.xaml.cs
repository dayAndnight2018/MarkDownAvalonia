using System;
using Windows.UI.Popups;
using ABI.Windows.UI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Markdown.Avalonia;
using MarkDownAvalonia.Controls;
using Color = Avalonia.Media.Color;
using MessageDialog = ABI.Windows.UI.Popups.MessageDialog;

namespace MarkDownAvalonia
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            this.FindControl<StackPanel>("postItemsPanel").Children.Add(new PostItemControl());
            InfoMessageBox messageBox = new InfoMessageBox();
            messageBox.Width = 480;
            messageBox.Height = 300;
            messageBox.ShowDialog(this);
            
            SuccessMessageBox success = new SuccessMessageBox();
            success.Width = 480;
            success.Height = 300;
            success.ShowDialog(this);
            
            WarningMessageBox warning = new WarningMessageBox();
            warning.Width = 480;
            warning.Height = 300;
            warning.ShowDialog(this);
            
            ErrorMessageBox errorMessageBox = new ErrorMessageBox();
            errorMessageBox.Width = 480;
            errorMessageBox.Height = 300;
            errorMessageBox.ShowDialog(this);
        }
        
        public void OnButtonClick(object sender, RoutedEventArgs e)
        {
            this.FindControl<Button>("btn").Content = "test";
            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowAsync(this);
        }

        public void OpenMenuClicked(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.ShowAsync(this);
        }

        public void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void OpenSettingWindow(Object sender, RoutedEventArgs e)
        {
            SettingWindow mb = new SettingWindow();
            mb.Width = 500;
            mb.Height = 320;
            mb.ShowDialog(this);
        }
        
        public void LabelMouseEntered(object sender, PointerEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(199,80,73));
            button.FontWeight = FontWeight.Bold;
        }
        
        public void LabelMouseLeave(object sender, PointerEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.FontWeight = FontWeight.Normal;
        }

        public void TbxKeyUp(object sender, KeyEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            this.FindControl<MarkdownScrollViewer>("md").Markdown = tbx.Text;
        }

    }
}