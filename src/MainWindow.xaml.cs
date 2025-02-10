using DiffClient.Commands;
using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffClient.Utility;
using DiffClient.Workflow;
using DiffEngine;
using DiffEngine.Win32.Cache;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#pragma warning disable

namespace DiffClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<Type,Type> keyValuePairs = new Dictionary<Type,Type>();
        private static MainWindow _mainWindow;
        private static MainWindowViewModel _mainWindowViewModel;

        internal HistoryFeature HistoryFeatureInstance { get; }
        public MenuItem OpenHistoryMenuItem { get; private set; } = new MenuItem() { Header = "OpenHistory" };
        public TabItem IndexRootTab { get; private set; }

        public ICommand g_ExitCommand { get; set; }

        public Queue<Action> TaskQueues = new Queue<Action>();

        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel(this);
            _mainWindowViewModel = this.DataContext as MainWindowViewModel;
            _mainWindow = this;
            keyValuePairs.Add(typeof(IndexPageView), typeof(IndexPageViewModel));
            keyValuePairs.Add(typeof(DiffDecompile), typeof(DiffDecompileViewModel));
            HistoryFeatureInstance = new HistoryFeature(this);
            this._setTitle();
            this.Closing += MainWindow_Closing;
            parseCommandLinesWorkflow();
        }
        private void _setTitle()
        {
            this.Title = $"{Const.Name} {Const.Version} {Const.Author}";
        }

        private void parseCommandLinesWorkflow()
        {
            var workflow = new WorkflowManager(this);
            if (workflow.Register().ErrorCode != 0)
            {
                throw new Exception("workflow register failed");
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            ExitCommand ec = g_ExitCommand as ExitCommand;
            ec.ExitFromClick(false);
        }

        private void menu0_Initialized(object sender, EventArgs e)
        {
            SetStatusException($"{nameof(MainWindow)}.{nameof(menu0_Initialized)}",LogStatusLevel.Info);
            MenuItem file = new MenuItem() { Header = "File" };

            file.Items.Add(new MenuItem() { Header = "Open", Command = new FileCommand(this) });
            file.Items.Add(OpenHistoryMenuItem);
            file.Items.Add(new MenuItem() { Header = "Clear All TabItem",Command = new DispatchCommand(this),CommandParameter = DispatchEvent.CleanTablItems });
            file.Items.Add(new MenuItem() { Header = "Setting",Command = new DispatchCommand(this),CommandParameter = DispatchEvent.OpenSetting });
            file.Items.Add(new MenuItem() { Header = "AccessCloud",Command = new DispatchCommand(this),CommandParameter = DispatchEvent.AccessCloud });
            HistoryFeatureInstance.initHistoryMenuItem();
            g_ExitCommand = new ExitCommand(this);
            file.Items.Add(new MenuItem() { Header = "Exit", Command = g_ExitCommand});
            this.menu0?.Items.Add(file);


            MenuItem help = new MenuItem() { Header = "Help" };
            help.Items.Add(new MenuItem() { Header = "About", Command = new AboutCommand(this) });
            this.menu0?.Items.Add(help);
        }

        private void rootTab_Initialized(object sender, EventArgs e)
        {
            SetStatusException($"{nameof(MainWindow)}.{nameof(rootTab_Initialized)}", LogStatusLevel.Info);
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(new Image() { Source = new BitmapImage(new Uri(@"./resources/indexpagetitle.png", UriKind.Relative)) });

            IndexRootTab = new TabItem()
            {
                Header = sp,
                Height = 20,
                Background = Brushes.White,
                Content = new IndexPageView(this),
            };
            IndexRootTab = IndexRootTab.SetDataContent<TabItem>(new IndexPageViewModel((IndexPageView)IndexRootTab.Content));
            this.rootTab?.TabControlAddAndSelect(IndexRootTab);
        }
        public static void SetStatusException(string msg, LogStatusLevel level = LogStatusLevel.Error)
        {
            if(_mainWindow?.g_status == null) { return; }
            // todo
            // status tooltips  two-ways binding
            _mainWindowViewModel.StatusToolTip = $"{msg}";
            switch (level)
            {
                case LogStatusLevel.Info:
                    _mainWindow.g_status.Foreground = Brushes.Gray;
                    break;
                case LogStatusLevel.Warning:
                    _mainWindow.g_status.Foreground = Brushes.Orange;
                    break;
                case LogStatusLevel.Error:
                case LogStatusLevel.Fatal:
                    _mainWindow.g_status.Foreground = Brushes.Red;
                    break;
                default:
                    break;
            }

            if(_mainWindowViewModel.GlobalLogStream != null)
            {
                string tmp = $"{DateTime.Now.ToShortDateString()} {level.ToString()} {msg}\n";
                byte[] data = Encoding.UTF8.GetBytes(msg);
                _mainWindowViewModel.GlobalLogStream.Write(data, 0, data.Length);
                _mainWindowViewModel.GlobalLogStream.Flush();
            }
        }

        internal void AddDiffDecompileToTreeViewFromUrl(DiffTreeItem father,string item)
        {
            try
            {
                if (item.EndsWith(".diffdecompile"))
                {
                    // todo cache from url
                    //if (cache)
                    //{
                    //    _mainWindow.HistoryFeatureInstance.AddCache(new HistoryCacheEntry(item));
                    //    _mainWindow.HistoryFeatureInstance.Update();
                    //}
                    if(father.Cloud.Initialized) { return; }

                    var diffd = new DiffDecompileManager();
                    var results = diffd.ParseFromUrl(item);
                    father.Entries = results;
                    if(father.Entries == null)
                    {
                        SetStatusException($"parse {item} diff entries was null");
                        father.Entries = new List<DiffDecompileEntry>();
                    }
                    father.Cloud.Initialized = true;
                    foreach (var entry in results)
                    {
                        father.Items.Add(new DiffTreeItem() { Header = $"{entry.PrimaryName}-{entry.SecondaryName}", Foreground = Brushes.Gray, DiffDecompileEntry = entry });
                    }
                }
                else
                {
                    _mainWindow.g_status.Content = $"{item} ext is not .diffdecompile during accesscloud";
                }
            }
            catch (Exception ex)
            {
                _mainWindow.g_status.Content = $"{item} {ex.Message} during accesscloud";
            }
        }

        public void AddDiffDecompileToTreeView(string item,bool cache = false)
        {
            try
            {
                if (item.EndsWith(".diffdecompile"))
                {
                    if (cache)
                    {
                        _mainWindow.HistoryFeatureInstance.AddCache(new HistoryCacheEntry(item));
                        _mainWindow.HistoryFeatureInstance.Update();
                    }

                    // parse diff decompile
                    var diffd = new DiffDecompileManager();
                    var results = diffd.ParseFromFile(item);
                    var father = new DiffTreeItem() { Header = $"{new FileInfo(item).Name}", Foreground = Brushes.Gray, DiffDecompileEntry = null,Cloud = new CloudModel { IsCloud = false, Initialized = true },Entries = results };
                    QueryObject.GetIndexTreeView(this).Items.Add(father);
                    // todo  lazy load  diff functions
                    foreach (var entry in results)
                    {
                        father.Items.Add(new DiffTreeItem() { Header = $"{entry.PrimaryName}-{entry.SecondaryName}", Foreground = Brushes.Gray, DiffDecompileEntry = entry });
                    }
                }
                else
                {
                    _mainWindow.g_status.Content = $"{item} ext is not .diffdecompile during drap-drop";
                }
            }
            catch (Exception ex)
            {
                _mainWindow.g_status.Content = $"{item} {ex.Message} during drap-drop";
            }
        }

        internal MainWindowViewModel mainWindowViewModel => _mainWindowViewModel;

        private void g_status_Initialized(object sender, EventArgs e)
        {
            //_mainWindow.g_status.Content = Process.GetCurrentProcess().StartInfo.Arguments;
        }

        private void rootTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if(rootTab.SelectedIndex != 0)
                    rootTab.SelectedIndex--;
                else
                    rootTab.SelectedIndex = rootTab.Items.Count - 1;
            }
            if (e.Key == Key.Right)
            {
                if (rootTab.SelectedIndex != rootTab.Items.Count - 1)
                    rootTab.SelectedIndex++;
                else
                    rootTab.SelectedIndex = 0;
            }
        }
        private void rootTab_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            ((TabItem)tab?.SelectedItem).Background = Brushes.LightYellow;
        }
    }
    public enum LogStatusLevel
    {
        Info,
        Warning, 
        Error,
        Fatal
    }
}