using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Markdown.Avalonia;
using MarkDownAvalonia.Controls;
using MarkDownAvalonia.Data;
using MarkDownAvalonia.Enums;
using MarkDownAvalonia.Extends;

namespace MarkDownAvalonia
{
    public class MainWindow : Window
    {
        // input text box
        private TextBox inputTbx;

        // search text box
        private TextBox searchBox;

        // container for posts
        private StackPanel articleListPanel;

        private Grid mainGrid;

        // previewer
        private MarkdownScrollViewer markdownPreview;

        // selected item
        private PostItemControl selectedItem = null;

        // cache posts for search
        private List<PostItemControl> cacheControls = new List<PostItemControl>();

        private static readonly string TIME_PATTERN = "yyyy/MM/dd HH:mm:ss";
        private static readonly string SUFFIX = ".md";

        private bool hidden = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new
            {
                Background = new SolidColorBrush(Color.Parse(CommonData.theme.Background)),
                Foreground = new SolidColorBrush(Color.Parse(CommonData.theme.Foreground)),
                Activecolor = new SolidColorBrush(Color.Parse(CommonData.theme.Activecolor))
            };
            // load controls
            this.inputTbx = this.FindControl<TextBox>("inputTextBox");
            this.searchBox = this.FindControl<TextBox>("searchBox");
            this.articleListPanel = this.FindControl<StackPanel>("postItemsPanel");
            this.markdownPreview = this.FindControl<MarkdownScrollViewer>("markdownPreview");
            this.mainGrid = this.FindControl<Grid>("mainGrid");
            // load config
            // todo: check config is null or not
            this.markdownPreview.AssetPathRoot = CommonData.config.PostDirectory;
            // load posts
            LoadPosts();
        }

        /// <summary>
        /// load post or reload posts
        /// </summary>
        private void LoadPosts()
        {
            // make sure directory exists
            GitUtils.MakeDirectory(CommonData.config.PostDirectory);
            // clear selected
            if (selectedItem != null)
            {
                selectedItem.RemoveHandlers();
                selectedItem = null;
            }

            inputTbx.Text = String.Empty;
            // clear
            articleListPanel.Children.Clear();

            // get markdown files
            IEnumerable<string> files = Directory.GetFiles(CommonData.config.PostDirectory)
                .Where(f => f.EndsWith(SUFFIX));
            // deal with each post
            foreach (var file in files)
            {
                PostItemControl current = new PostItemControl(file, this.inputTbx);
                current.Register((sender, e) =>
                {
                    foreach (PostItemControl control in cacheControls)
                    {
                        control.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
                        control.Foreground = new SolidColorBrush(Colors.Silver);
                        control.RemoveHandlers();
                    }

                    // current 
                    current.Background = new SolidColorBrush(Color.FromRgb(199, 80, 73));
                    current.Foreground = new SolidColorBrush(Colors.White);
                    selectedItem = current;
                    current.configTimer();
                });
                articleListPanel.Children.Add(current);
            }

            // bake in cache
            IControl[] array = new IControl[articleListPanel.Children.Count];
            articleListPanel.Children.CopyTo(array, 0);
            // add cache
            cacheControls.Clear();
            foreach (var item in array)
            {
                cacheControls.Add(item as PostItemControl);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// add new post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewPost(object sender, RoutedEventArgs e)
        {
            // todo : check config
            // there already a temp post
            if (findTempFile())
                return;

            PostItemControl current = new PostItemControl(this.inputTbx);
            current.Register((sender, e) =>
            {
                foreach (PostItemControl control in cacheControls)
                {
                    control.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
                    control.Foreground = new SolidColorBrush(Colors.Silver);
                    control.RemoveHandlers();
                }

                current.Background = new SolidColorBrush(Color.FromRgb(199, 80, 73));
                current.Foreground = new SolidColorBrush(Colors.White);
                selectedItem = current;
                current.configTimer();
            });
            inputTbx.Text =
                $"---{Environment.NewLine}{Environment.NewLine}title: {Environment.NewLine}{Environment.NewLine}date: {DateTime.Now.ToString(TIME_PATTERN)}{Environment.NewLine}{Environment.NewLine}tags: {Environment.NewLine}{Environment.NewLine}---{Environment.NewLine}{Environment.NewLine}";
            markdownPreview.Markdown =
                $"---{Environment.NewLine}{Environment.NewLine}title: {Environment.NewLine}{Environment.NewLine}date: {DateTime.Now.ToString(TIME_PATTERN)}{Environment.NewLine}{Environment.NewLine}tags: {Environment.NewLine}{Environment.NewLine}---{Environment.NewLine}{Environment.NewLine}";
            current.updateCache();
            articleListPanel.Children.Insert(0, current);
            cacheControls.Add(current);
            SelectControl(current);
        }

        /// <summary>
        /// save posts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SavePost(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
                return;

            // current selected item is local not temp
            if (selectedItem.isExists)
            {
                // exists
                using (FileStream sw = new FileStream(selectedItem.info.FullName, FileMode.Create))
                {
                    sw.Write(Encoding.UTF8.GetBytes(inputTbx.Text));
                    sw.Flush();
                }

                await MessageBox.showSuccess(this, "Saved success!");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExtension = SUFFIX;
            saveFileDialog.Directory = CommonData.config.PostDirectory;
            string result = await saveFileDialog.ShowAsync(this);
            if (!String.IsNullOrWhiteSpace(result))
            {
                // auto detect suffix
                if (!result.ToLower().EndsWith(SUFFIX))
                {
                    result = result + SUFFIX;
                }

                // file name already exists
                if (File.Exists(result))
                {
                    await MessageBox.showError(this, "File is already exists!");
                }
                else
                {
                    // save file
                    using (FileStream fs = new FileStream(result, FileMode.Create))
                    {
                        fs.Write(Encoding.UTF8.GetBytes(this.inputTbx.Text));
                        fs.Flush();
                    }

                    // deal for present
                    string fileName = Path.GetFileNameWithoutExtension(result);
                    selectedItem.updateItemPresent(fileName);
                    selectedItem.isExists = true;
                    selectedItem.info = new FileInfo(result);
                    await MessageBox.showSuccess(this, "Saved success!");
                }
            }
            else
            {
                await MessageBox.showError(this, "File name could not be null or empty!");
            }
        }

        /// <summary>
        /// reload posts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReloadDirectory(object sender, RoutedEventArgs e)
        {
            LoadPosts();
        }

        /// <summary>
        /// pull source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PullFiles(object sender, RoutedEventArgs e)
        {
            GitUtils.GitPull();
            Console.WriteLine("pull over");
        }

        /// <summary>
        /// push source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PushFiles(object sender, RoutedEventArgs e)
        {
            GitUtils.GitPush();
            Console.WriteLine("push over");
        }

        public void PreviewPost(object sender, RoutedEventArgs e)
        {
            FindWindow mb = new FindWindow(inputTbx);
            mb.Width = 500;
            mb.Height = 320;
            mb.Show(this);
        }

        /// <summary>
        /// publish posts to website
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PublishPost(object sender, RoutedEventArgs e)
        {
            GitUtils.HexoDeploy();
            Console.WriteLine("publish over");
        }

        /// <summary>
        /// clone source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RestorePost(object sender, RoutedEventArgs e)
        {
            GitUtils.GitRestore();
            Console.WriteLine("restore over");
        }

        /// <summary>
        /// delete post
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void DiscardPost(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null)
                return;

            if (selectedItem.isExists)
            {
                if (!File.Exists(selectedItem.info.FullName))
                {
                    await MessageBox.showError(this, "File not exists!");
                }

                // file exists
                if (await MessageBox.showWarnning(this, "Deleting file, continue?"))
                {
                    // delete from disk
                    File.Delete(selectedItem.info.FullName);

                    RemoveItemInPanel();

                    await MessageBox.showSuccess(this, "Delete success!");
                }
            }
            else
            {
                if (await MessageBox.showWarnning(this, "Deleting file, continue?"))
                {
                    RemoveItemInPanel();
                }
            }
        }

        /// <summary>
        /// remove post item in panel
        /// </summary>
        private void RemoveItemInPanel()
        {
            // remove event handler
            selectedItem.RemoveHandlers();
            // remove 
            this.articleListPanel.Children.Remove(selectedItem);
            // remove cache
            cacheControls.TryRemove(selectedItem);
            // remove selected
            selectedItem = null;
            // clear input text box
            inputTbx.Text = String.Empty;
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
            mb.Show(this);
        }

        public void ToggleListPanel(Object sender, RoutedEventArgs e)
        {
            mainGrid.ColumnDefinitions[0].Width = !hidden ? new GridLength(0) : new GridLength(this.Width / 5);
            hidden = !hidden;
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
                Vector old = markdownPreview.ScrollValue;
                markdownPreview.Markdown = this.inputTbx.Text;
                markdownPreview.ScrollValue = new Vector(old.X, old.Y);
            }
        }

        /// <summary>
        /// 查找是否存在临时文件
        /// </summary>
        /// <returns></returns>
        private bool findTempFile()
        {
            return cacheControls.Exists(ele => !ele.isExists);
        }

        /// <summary>
        /// 选中指定项
        /// </summary>
        /// <param name="target"></param>
        private void SelectControl(PostItemControl target)
        {
            foreach (PostItemControl control in this.cacheControls)
            {
                if (control == target)
                {
                    control.RemoveHandlers();
                    control.Background = new SolidColorBrush(Color.FromRgb(199, 80, 73));
                    control.Foreground = new SolidColorBrush(Colors.White);
                    selectedItem = control;
                    control.configTimer();
                }
                else
                {
                    control.Background = new SolidColorBrush(Color.FromRgb(49, 47, 47));
                    control.Foreground = new SolidColorBrush(Colors.Silver);
                    control.RemoveHandlers();
                }
            }
        }


        public async void InputShortcutKeys(object sender, KeyEventArgs e)
        {
            KeyModifiers modifiers = e.KeyModifiers;

            Key key = e.Key;
            // control + v
            if (modifiers == KeyModifiers.Control && key == Key.V)
            {
                String[] format = await Application.Current.Clipboard.GetFormatsAsync();
                if (format.Length > 0)
                {
                    if (format[0].Equals("public.png") && selectedItem != null && selectedItem.isExists)
                    {
                        IClipboard clipboard = Application.Current.Clipboard;
                        byte[] data = await clipboard.GetDataAsync(format[1]) as byte[];
                        Stream stream = new MemoryStream(data);
                        Bitmap image = new Bitmap(stream);
                        string path = Path.Combine(CommonData.config.PostDirectory,
                            Path.GetFileNameWithoutExtension(selectedItem.info.FullName));
                        GitUtils.MakeDirectory(path);
                        string fileName = Guid.NewGuid().ToString() + ".png";
                        string filePath = Path.Combine(Path.GetFileNameWithoutExtension(selectedItem.info.FullName),
                            fileName);
                        image.Save(Path.Combine(path, fileName));
                        this.inputTbx.Text =
                            this.inputTbx.Text.Insert(this.inputTbx.CaretIndex, $"![image]({filePath})");
                    }
                }

                return;
            }

// tab
            if (key == Key.Tab)
            {
                e.Handled = true;
                this.inputTbx.Text = inputTbx.Text.Insert(this.inputTbx.CaretIndex, "        ");
                return;
            }

            String selectedText = this.inputTbx.SelectedText;

            Dictionary<Key, Tag> dic = new Dictionary<Key, Tag>();
            dic.Add(Key.D1, TagCollection.H1);
            dic.Add(Key.D2, TagCollection.H2);
            dic.Add(Key.D3, TagCollection.H3);
            dic.Add(Key.D4, TagCollection.H4);
            dic.Add(Key.D5, TagCollection.H5);
            dic.Add(Key.D6, TagCollection.H6);

            if (modifiers == KeyModifiers.Control && dic.ContainsKey(key))
            {
                HandleHeaderShortCutKeys(dic[key]);
            }

            if (modifiers == KeyModifiers.Control && key == Key.S)
            {
                SavePost(null, null);
            }
        }

        public void InputKeyDown(object sender, KeyEventArgs e)
        {
            KeyModifiers modifiers = e.KeyModifiers;

            Key key = e.Key;
            if (key == Key.Tab)
            {
                e.Handled = true;
                return;
            }
        }

        private void HandleHeaderShortCutKeys(Tag tag)
        {
            String selectedText = inputTbx.SelectedText;
            if (String.IsNullOrWhiteSpace(selectedText))
            {
                return;
            }

            if (selectedText.StartsWith(tag.Prefix))
            {
                this.inputTbx.SelectedText = selectedText.Substring(tag.Prefix.Length);
            }
            else
            {
                this.inputTbx.SelectedText = String.Concat(tag.Prefix, selectedText);
            }
        }


        public void SearchBoxKeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            if (key == Key.Return)
            {
                if (String.IsNullOrWhiteSpace(searchBox.Text))
                {
                    articleListPanel.Children.Clear();
                    foreach (var item in cacheControls)
                    {
                        articleListPanel.Children.Add(item);
                    }

                    return;
                }

                List<PostItemControl> filtered =
                    cacheControls.Where(c => c.GetName().ToLower().Contains(searchBox.Text.ToLower())).ToList();
                articleListPanel.Children.Clear();
                if (filtered.Count > 0)
                {
                    foreach (var item in filtered)
                    {
                        articleListPanel.Children.Add(item);
                    }
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// 处理窗体大小改变
        /// </summary>
        /// <param name="state"></param>
        protected override void HandleWindowStateChanged(WindowState state)
        {
            mainGrid.ColumnDefinitions[0].Width = new GridLength(this.Width / 5);
            base.HandleWindowStateChanged(state);
        }
    }
}