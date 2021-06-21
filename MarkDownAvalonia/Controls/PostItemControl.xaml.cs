using System;
using System.IO;
using System.Text;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace MarkDownAvalonia.Controls
{
    /// <summary>
    /// use self defined control
    /// </summary>
    public class PostItemControl : UserControl
    {
        /// <summary>
        /// item control
        /// </summary>
        private TextBlock itemTitleTbk, itemTimeTbk;
        
        /// <summary>
        /// file info for item
        /// </summary>
        public FileInfo info = null;
        
        /// <summary>
        /// text box for input
        /// </summary>
        private readonly TextBox inputTbx;
        
        /// <summary>
        /// auto save timer
        /// </summary>
        private Timer timer;
        
        /// <summary>
        /// cache
        /// </summary>
        private string cache = string.Empty;
        
        /// <summary>
        /// if exist
        /// </summary>
        public bool isExists = false;

        private const string DatePattern = "yyyy/MM/dd HH:mm:ss";
        private const string DefaultTitle = "md*";

        public PostItemControl()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public PostItemControl(TextBox inputTbx)
        {
            AvaloniaXamlLoader.Load(this);
            Init();
            
            this.inputTbx = inputTbx;
            itemTitleTbk.Text = DefaultTitle;
            itemTimeTbk.Text = DateTime.Now.ToString(DatePattern);
        }

        public string GetName()
        {
            return itemTitleTbk.Text;
        }

        public PostItemControl(string file, TextBox inputTbx)
        {
            AvaloniaXamlLoader.Load(this);
            this.inputTbx = inputTbx;
            info = new FileInfo(file);

            Init();

            if (info.Exists)
            {
                var name = Path.GetFileNameWithoutExtension(info.FullName);
                var date = info.LastWriteTime.ToString(DatePattern);
                this.itemTitleTbk.Text = name;
                this.itemTimeTbk.Text = date;
                isExists = true;
            }
        }

        public void UpdateItemPresent(string name)
        {
            this.itemTitleTbk.Text = name;
            this.itemTimeTbk.Text = DateTime.Now.ToString(DatePattern);
        }

        public void UpdateCache()
        {
            this.cache = this.inputTbx.Text;
        }

        private void Init()
        {
            // get controls
            this.itemTitleTbk = this.FindControl<TextBlock>("itemTitleTbk");
            this.itemTimeTbk = this.FindControl<TextBlock>("itemTimeTbk");
        }

        public void ConfigTimer()
        {
            // config timer
            timer = new Timer(30 * 1000);
            timer.Elapsed += Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        public void Elapsed(object sender, ElapsedEventArgs e)
        {
            // reduce write times
            if (isExists)
            {
                if (timer != null && timer.Enabled && info != null && !cache.Equals(inputTbx.Text))
                {
                    if (inputTbx.Text.StartsWith(cache))
                    {
                        using (var sw = new FileStream(info.FullName, FileMode.Append))
                        {
                            sw.Write(Encoding.UTF8.GetBytes(inputTbx.Text.Substring(cache.Length)));
                            sw.Flush();
                        }
                    }
                    else
                    {
                        using (var sw = new FileStream(info.FullName, FileMode.Create))
                        {
                            sw.Write(Encoding.UTF8.GetBytes(inputTbx.Text));
                            sw.Flush();
                        }
                    }

                    cache = inputTbx.Text;
                }
            }
            else
            {
                cache = inputTbx.Text;
            }
        }

        public void Register(EventHandler<PointerPressedEventArgs> handler)
        {
            this.PointerPressed += handler;
        }

        public void RemoveHandlers()
        {
            this.PointerEnter -= this.GetFocus;
            this.PointerLeave -= this.LostFocus;
            if (timer != null)
            {
                this.timer.Enabled = false;
                this.timer.Stop();
                this.timer = null;
            }
        }

        /// <summary>
        /// item effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GetFocus(object sender, PointerEventArgs e)
        {
            this.Background = new SolidColorBrush(Color.FromRgb(199, 80, 73));
            this.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// item effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LostFocus(object sender, PointerEventArgs e)
        {
            this.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
            this.Foreground = new SolidColorBrush(Colors.Silver);
        }

        public void Clicked(object sender, PointerPressedEventArgs e)
        {
            if (isExists)
            {
                // file exists, read file
                var text = Read(info.FullName);
                inputTbx.Text = text;
                cache = text;
            }
            else
            {
                inputTbx.Text = cache;
            }

            ConfigTimer();
        }

        /// <summary>
        /// read text from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string Read(string path)
        {
            var text = string.Empty;
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                text = sr.ReadToEnd();
            }

            return text;
        }
    }
}