using System;
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
            // control
            AvaloniaXamlLoader.Load(this);
            this.rootDirectoryTbx = this.FindControl<TextBox>("rootDirectoryTbx");
            this.gitAddressTbx = this.FindControl<TextBox>("gitAddressTbx");
            
            // config
            this.config = CommonData.config;
            
            // load config
            if (config != null)
            {
                this.rootDirectoryTbx.Text = config.RootDirectory ?? string.Empty;
                this.gitAddressTbx.Text = config.GitAddress ?? string.Empty;
            }
        }

        /**
         * close window
         */
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /**
         * choose directory
         */
        public async void selectDir(Object sender, GotFocusEventArgs e)
        {
            var task = await new OpenFolderDialog().ShowAsync(this);
            var tbx = sender as TextBox;
            tbx.Text = task;
        }
    }
}