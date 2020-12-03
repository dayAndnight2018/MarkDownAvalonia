using System;
using ABI.Windows.UI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

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
        
        public void LabelMouseEntered(object sender, PointerEventArgs e)
        {
            Label label = sender as Label;
            label.Background = new SolidColorBrush(Colors.Gray);
        }
        
        public void LabelMouseLeave(object sender, PointerEventArgs e)
        {
            Label label = sender as Label;
            label.Background = new SolidColorBrush(Colors.Transparent);
        }
        
        
    }
}