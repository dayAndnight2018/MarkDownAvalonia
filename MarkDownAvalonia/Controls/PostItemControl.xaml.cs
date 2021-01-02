using System;
using System.IO;
using System.Text;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Timer = System.Timers.Timer;

namespace MarkDownAvalonia.Controls
{
    public class PostItemControl : UserControl
    {
        private TextBlock itemTitleTbk, itemTimeTbk;
        public FileInfo info = null;
        private TextBox inputTbx;
        private Timer timer;
        private string cache = String.Empty;
        public bool isExists = false;
        
        public PostItemControl()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        public PostItemControl(TextBox inputTbx)
        {
            AvaloniaXamlLoader.Load(this);
            this.inputTbx = inputTbx;
            init();
            this.itemTitleTbk.Text = "md*";
            this.itemTimeTbk.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public string GetName()
        {
            return this.itemTitleTbk.Text;
        }
        
        public PostItemControl(string file, TextBox inputTbx)
        {
            AvaloniaXamlLoader.Load(this);
            this.inputTbx = inputTbx;
            info = new FileInfo(file);
            
            init();

            if (info.Exists)
            {
                string name = Path.GetFileNameWithoutExtension(info.FullName);
                string date = info.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                this.itemTitleTbk.Text = name;
                this.itemTimeTbk.Text = date;
                isExists = true;
            }
        }

        public void updateItemPresent(String name)
        {
            this.itemTitleTbk.Text = name;
            this.itemTimeTbk.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public void updateCache()
        {
            this.cache = this.inputTbx.Text;
        }

        private void init()
        {
            // get controls
            this.itemTitleTbk = this.FindControl<TextBlock>("itemTitleTbk");
            this.itemTimeTbk = this.FindControl<TextBlock>("itemTimeTbk");
        }

        public void configTimer()
        {
            // config timer
            timer = new Timer(30 * 1000);
            timer.Elapsed += Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        public void Elapsed(Object sender, ElapsedEventArgs e)
        {
            // reduce write times
            if (isExists)
            {
                if (timer != null && timer.Enabled && info != null && !cache.Equals(inputTbx.Text))
                {
                    if (inputTbx.Text.StartsWith(cache))
                    {
                        using (FileStream sw = new FileStream(info.FullName, FileMode.Append))
                        {
                            sw.Write(Encoding.UTF8.GetBytes(inputTbx.Text.Substring(cache.Length)));
                            sw.Flush();
                        }
                    }
                    else
                    {
                        using (FileStream sw = new FileStream(info.FullName, FileMode.Create))
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

        public void Remove()
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
        public void GetFocus(Object sender, PointerEventArgs e)
        {
            this.Background = new SolidColorBrush(Color.FromRgb(199,80,73));
            this.Foreground = new SolidColorBrush(Colors.White);
        }
        
        /// <summary>
        /// item effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LostFocus(Object sender, PointerEventArgs e)
        {
            this.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
            this.Foreground = new SolidColorBrush(Colors.Silver);
        }

        public void Clicked(Object sender, PointerPressedEventArgs e)
        {
            if (isExists)
            {
                // file exists, read file
                string text = read(info.FullName);
                inputTbx.Text = text;
                cache = text;
            }
            else
            {
                inputTbx.Text = cache;
            }
            
            configTimer();
        }

        /// <summary>
        /// read text from a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string read(String path)
        {
            string text = String.Empty;
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                text = sr.ReadToEnd();
            }

            return text;
        }
    }
}