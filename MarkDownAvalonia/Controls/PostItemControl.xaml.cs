using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace MarkDownAvalonia.Controls
{
    public class PostItemControl : UserControl
    {
        public PostItemControl()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void GetFocus(Object sender, PointerEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Silver);
        }
        
        public void LostFocus(Object sender, PointerEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
        }
        
    }
}