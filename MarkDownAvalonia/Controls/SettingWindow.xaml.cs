using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarkDownAvalonia.Data;

namespace MarkDownAvalonia.Controls
{
    public class SettingWindow : Window
    {
        private TextBox gitAddressTbx;
        private TextBox rootDirectoryTbx;
        private Configuration config;
        
        public SettingWindow()
        {
            AvaloniaXamlLoader.Load(this);
            this.rootDirectoryTbx = this.FindControl<TextBox>("rootDirectoryTbx");
            this.gitAddressTbx = this.FindControl<TextBox>("gitAddressTbx");
            this.config = CommonData.config;
            if (config != null)
            {
                this.rootDirectoryTbx.Text = config.RootDirectory??String.Empty;
                this.gitAddressTbx.Text = config.GitAddress??String.Empty;
            }
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