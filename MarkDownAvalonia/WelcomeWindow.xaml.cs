using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Markdown.Avalonia;
using MarkDownAvalonia.Controls;
using Color = Avalonia.Media.Color;

namespace MarkDownAvalonia
{
    public class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            SettingWindow mb = new SettingWindow();
            //SaveFileDialog sfd = new SaveFileDialog();
            mb.Width = 640;
            mb.Height = 320;
            mb.ShowDialog(this);
            //sfd.ShowAsync(this);
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

        public void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
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