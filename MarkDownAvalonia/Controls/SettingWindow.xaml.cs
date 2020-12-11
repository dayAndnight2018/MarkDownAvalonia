using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MarkDownAvalonia.Controls
{
    public class SettingWindow : Window
    {

        public SettingWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public async void selectDir(Object sender, GotFocusEventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();
            string task = await ofd.ShowAsync(this);
            TextBox tbx = sender as TextBox;
            tbx.Text = task;
        }
    }
}