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
        private double oldGridWidth = 0;

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
                current.Register(new EventHandler<PointerPressedEventArgs>((sender, e) =>
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
                }));
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
            if (findTempFile() != null)
            {
                return;
            }

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
            {
                return;
            }

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
                // file exists
                if (File.Exists(selectedItem.info.FullName))
                {
                    bool result = await MessageBox.showWarnning(this, "Deleting file, continue?");
                    if (result)
                    {
                        // stop auto save
                        selectedItem.RemoveHandlers();
                        // delete from dosk
                        File.Delete(selectedItem.info.FullName);
                        // remove 
                        this.articleListPanel.Children.Remove(selectedItem);
                        if (cacheControls.Contains(selectedItem))
                        {
                            cacheControls.Remove(selectedItem);
                        }

                        // remove selected
                        selectedItem = null;
                        // clear input text box
                        inputTbx.Text = String.Empty;
                        await MessageBox.showSuccess(this, "Delete success!");
                    }
                }
                else
                {
                    await MessageBox.showError(this, "File not exists!");
                }
            }
            else
            {
                if (await MessageBox.showWarnning(this, "Deleting file, continue?"))
                {
                    // remove event handler
                    selectedItem.RemoveHandlers();
                    // remove item in panel
                    this.articleListPanel.Children.Remove(selectedItem);
                    // try remove in cache
                    cacheControls.TryRemove(selectedItem);
                    
                    // clear resource and input area
                    selectedItem = null;
                    inputTbx.Text = string.Empty;
                }
            }
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
            if (!hidden)
            {
                oldGridWidth = mainGrid.ColumnDefinitions[0].ActualWidth;
                mainGrid.ColumnDefinitions[0].Width = new GridLength(0);
            }
            else
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(oldGridWidth);
            }

            hidden = !hidden;
        }

        /// <summary>
        /// button effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LabelMouseEntered(object sender, PointerEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
            button.Foreground = new SolidColorBrush(Color.FromRgb(199, 80, 73));
            button.FontWeight = FontWeight.Bold;
        }

        /// <summary>
        /// button effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LabelMouseLeave(object sender, PointerEventArgs e)
        {
            Button button = sender as Button;
            // button.Background = new SolidColorBrush(Colors.Transparent);
            button.Foreground = new SolidColorBrush(Colors.Transparent);
            button.FontWeight = FontWeight.Normal;
        }

        public void PathIconMouseEnter(object sender, PointerEventArgs e)
        {
            PathIcon icon = sender as PathIcon;
            icon.Foreground = new SolidColorBrush(Color.FromRgb(199, 80, 73));
            icon.FontWeight = FontWeight.Bold;
        }

        public void PathIconMouseLeave(object sender, PointerEventArgs e)
        {
            PathIcon icon = sender as PathIcon;
            icon.Foreground = new SolidColorBrush(Colors.White);
            icon.FontWeight = FontWeight.Normal;
        }

        public void ContextMenuPointerEnter(object sender, PointerEventArgs e)
        {
            ContextMenu icon = sender as ContextMenu;
            icon.Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
            // icon.Foreground = 
            // icon.FontWeight = FontWeight.Bold;
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

        private PostItemControl findTempFile()
        {
            foreach (PostItemControl control in cacheControls)
            {
                if (!control.isExists)
                {
                    return control;
                }
            }

            return null;
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
                this.inputTbx.Text =
                    this.inputTbx.Text.Insert(this.inputTbx.CaretIndex, "        ");
                return;
            }

            String selectedText = this.inputTbx.SelectedText;
            // control + 1
            if (modifiers == KeyModifiers.Control && key == Key.D1)
            {
                if (String.IsNullOrWhiteSpace(selectedText))
                {
                    return;
                }

                if (selectedText.StartsWith("# "))
                {
                    this.inputTbx.SelectedText = selectedText.Substring("# ".Length);
                }
                else
                {
                    this.inputTbx.SelectedText = String.Concat("# ", selectedText);
                }
            }

            // control + 2
            if (modifiers == KeyModifiers.Control && key == Key.D2)
            {
                if (String.IsNullOrWhiteSpace(selectedText))
                {
                    return;
                }

                if (selectedText.StartsWith("## "))
                {
                    this.inputTbx.SelectedText = selectedText.Substring("## ".Length);
                }
                else
                {
                    this.inputTbx.SelectedText = String.Concat("## ", selectedText);
                }
            }

            // control + 3
            if (modifiers == KeyModifiers.Control && key == Key.D3)
            {
                if (String.IsNullOrWhiteSpace(selectedText))
                {
                    return;
                }

                if (selectedText.StartsWith("### "))
                {
                    this.inputTbx.SelectedText = selectedText.Substring("### ".Length);
                }
                else
                {
                    this.inputTbx.SelectedText = String.Concat("### ", selectedText);
                }
            }

            // control + 4
            if (modifiers == KeyModifiers.Control && key == Key.D4)
            {
                if (String.IsNullOrWhiteSpace(selectedText))
                {
                    return;
                }

                if (selectedText.StartsWith("#### "))
                {
                    this.inputTbx.SelectedText = selectedText.Substring("#### ".Length);
                }
                else
                {
                    this.inputTbx.SelectedText = String.Concat("#### ", selectedText);
                }
            }

            // control + 5
            if (modifiers == KeyModifiers.Control && key == Key.D5)
            {
                if (String.IsNullOrWhiteSpace(selectedText))
                {
                    return;
                }

                if (selectedText.StartsWith("##### "))
                {
                    this.inputTbx.SelectedText = selectedText.Substring("##### ".Length);
                }
                else
                {
                    this.inputTbx.SelectedText = String.Concat("##### ", selectedText);
                }
            }

            // control + 6
            if (modifiers == KeyModifiers.Control && key == Key.D6)
            {
                if (String.IsNullOrWhiteSpace(selectedText))
                {
                    return;
                }

                if (selectedText.StartsWith("###### "))
                {
                    this.inputTbx.SelectedText = selectedText.Substring("###### ".Length);
                }
                else
                {
                    this.inputTbx.SelectedText = String.Concat("###### ", selectedText);
                }
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
    }
}