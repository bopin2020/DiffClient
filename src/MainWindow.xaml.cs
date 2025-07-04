using DiffClient.Commands;
using DiffClient.DataModel;
using DiffClient.Services;
using DiffClient.UserControls;
using DiffClient.Utility;
using DiffClient.Workflow;
using DiffDecompile.Core;
using DiffEngine.Win32.Cache;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        #region Private Members

        private static MainWindow _mainWindow;
        private static MainWindowViewModel _mainWindowViewModel;

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
        private void parseHistoryWorkflow()
        {
            var workflow = new WorkflowManager(this);
            workflow.InitTimer();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            ExitCommand ec = g_ExitCommand as ExitCommand;
            if (ec.ExitFromClick(false))
            {
                e.Cancel = true;
            }
        }

        private void initMenu()
        {
            HistoryFeatureInstance = new HistoryFeature(this);

            SetStatusException($"{nameof(MainWindow)}.{nameof(menu0_Initialized)}", LogStatusLevel.Info);
            MenuItem file = new MenuItem() { Header = "File" };
            file.Items.Add(new MenuItem() { Header = "Open", Command = new FileCommand(this) });
            file.Items.Add(new MenuItem() { Header = "Open Workdir", Command = new FileCommand(this),CommandParameter = true });
            OpenHistoryMenuItem = new MenuItem() { Header = "OpenHistory" };
            file.Items.Add(OpenHistoryMenuItem);
            file.Items.Add(new MenuItem() { Header = "Clear All TabItem", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.CleanTablItems });
            file.Items.Add(new MenuItem() { Header = "Setting", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.OpenSetting });
            file.Items.Add(new MenuItem() { Header = "Log", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.OpenLog });
            file.Items.Add(new MenuItem() { Header = "Access Local", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.AccessLocalStore });
            file.Items.Add(new MenuItem() { Header = "AccessCloud", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.AccessCloud });
            this.TaskQueues.Enqueue(() => HistoryFeatureInstance.initHistoryMenuItem());
            g_ExitCommand = new ExitCommand(this);
            file.Items.Add(new MenuItem() { Header = "Exit", Command = g_ExitCommand });
            file.Items.Add(new MenuItem() { Header = "Restart", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.Restart });
            this.menu0?.Items.Add(file);

            MenuItem features = new MenuItem() { Header = "Feature" };
            features.Items.Add(new MenuItem() { Header = "Dynamic grid", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.Dynamic });
            features.Items.Add(new MenuItem() { Header = "Job Manager", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.JobManager });
            features.Items.Add(new MenuItem() { Header = "Clear History", Command = new DispatchCommand(this), CommandParameter = DispatchEvent.ClearHistories });
            this.menu0?.Items.Add(features);

            MenuItem help = new MenuItem() { Header = "Help" };
            help.Items.Add(new MenuItem() { Header = "About", Command = new AboutCommand(this) });
            this.menu0?.Items.Add(help);
        }

        private void menu0_Initialized(object sender, EventArgs e)
        {
            initMenu();
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

        private void g_status_Initialized(object sender, EventArgs e)
        {
        }

        private void rootTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (rootTab.SelectedIndex != 0)
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
            if ((TabItem)tab?.SelectedItem != null)
                ((TabItem)tab?.SelectedItem).Background = Brushes.LightYellow;
        }

        #endregion

        #region Internal Members

        internal HistoryFeature HistoryFeatureInstance { get; private set; }

        internal MainWindowViewModel mainWindowViewModel { get; set; }

        internal void AddDiffDecompileToTreeViewFromUrl(DiffTreeItem father, string item, bool cache = false)
        {
            try
            {
                if (item.EndsWith(".diffdecompile"))
                {
                    // todo cache from url
                    if (cache)
                    {
                        _mainWindow.HistoryFeatureInstance.AddCache(new HistoryCacheEntry(item));
                        _mainWindow.HistoryFeatureInstance.Update();
                    }
                    if (father.Cloud.Initialized) { return; }

                    var diffd = new DiffDecompileManager();
                    var results = diffd.ParseFromUrl(item);
                    father.Entries = results;
                    if (father.Entries == null)
                    {
                        SetStatusException($"parse {item} diff entries was null");
                        father.Entries = new List<DiffDecompileEntry>();
                    }
                    father.Cloud.Initialized = true;
                    foreach (var entry in results)
                    {
                        var tmp = new DiffTreeItem() { Header = $"{entry.PrimaryName}-{entry.SecondaryName}", Foreground = Brushes.Gray, DiffDecompileEntry = entry };
                        father.Items.Add(tmp);
                        father.Children.Add(tmp);
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

        #endregion

        #region Public Members

        public Dictionary<Type, Type> keyValuePairs = new Dictionary<Type, Type>();

        public Dictionary<string, int> DiffDecompileCache = new Dictionary<string, int>();

        public MenuItem OpenHistoryMenuItem { get; private set; }
        public TabItem IndexRootTab { get; private set; }

        public ICommand g_ExitCommand { get; set; }

        public Queue<Action> TaskQueues = new Queue<Action>();

        public static void SetStatusException(string msg, LogStatusLevel level = LogStatusLevel.Error)
        {
            //if (_mainWindow?.progressBar1 != null)
            //    using (var tmp = new AsyncTaskInternal(_mainWindow.progressBar1));

            if (_mainWindow?.g_status == null) { return; }
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
            string tmp = $"{DateTime.Now.ToShortDateString()} {level.ToString()} {msg}";
            MainWindowViewModel.PushStrWithGuard(tmp);
            if (_mainWindowViewModel.GlobalLogStream != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                _mainWindowViewModel.GlobalLogStream.Write(data, 0, data.Length);
                _mainWindowViewModel.GlobalLogStream.Flush();
            }

            _mainWindowViewModel.xxxBuilderService.JobContext.Message = msg;
            _mainWindowViewModel.xxxBuilderService.ProcessReporter.ReportProgress(_mainWindowViewModel.xxxBuilderService.JobContext);
        }

        public void AddDirectoryToTreeView(string dir)
        {
            if (DiffDecompileCache.ContainsKey(dir))
            {
                SetStatusException($"has contain {dir}", LogStatusLevel.Warning);
                return;
            }
            DiffDecompileCache.Add(dir, 0);
            var root = new DiffTreeItem()
            {
                Header = "@root",
                IsLocal = true,
                FullPath = dir,
                Foreground = Brushes.DarkGray,
                Type = TreeItemType.LocalRoot,
            };
            QueryObject.GetIndexPageViewContent(_mainWindow).ViewModel.TreeItemsSource.Add(root);

            foreach (var item in Directory.GetDirectories(dir))
            {
                root.AddProxy(new DiffTreeItem()
                {
                    Header = new DirectoryInfo(item).Name,
                    FullPath = item,
                    IsLocal = true,
                    Foreground = Brushes.DarkGray,
                    Type = TreeItemType.LocalDirectory,
                });
            }
        }

        internal void AddDiffDecompileToTreeView(string item, bool cache = false, DiffTreeItem father = null)
        {
            try
            {
                if (item.EndsWith(".diffdecompile"))
                {
                    if (DiffDecompileCache.ContainsKey(item))
                    {
                        SetStatusException("has contain it", LogStatusLevel.Warning);
                        return;
                    }
                    DiffDecompileCache.Add(item, 0);
                    if (cache)
                    {
                        _mainWindow.HistoryFeatureInstance.AddCache(new HistoryCacheEntry(item),hard: true);
                    }

                    // parse diff decompile
                    var diffd = new DiffDecompileManager();
                    var results = diffd.ParseFromFile(item);
                    if(father == null)
                        father = new DiffTreeItem()
                        {
                            Header = $"{new FileInfo(item).Name}",
                            IsLocal = true,
                            OS = "",
                            Date = "",
                            Foreground = Brushes.Gray,
                            DiffDecompileEntry = null,
                            Cloud = new CloudModel
                            {
                                IsCloud = false,
                                Initialized = true
                            },
                            Entries = results
                        };
                    else
                    {
                        father.Entries = results;
                    }

                    father.ToolTip = new TreeToolTipBuilder(new FileInfo(item).Name).ToString();
                    father.IsExpanded = true;

                    if(father.Parent == null)
                        QueryObject.GetIndexPageViewContent(this).ViewModel.TreeItemsSource.Add(father);
                    // todo  lazy load  diff functions
                    foreach (var entry in results)
                    {
                        father.AddProxy(new DiffTreeItem()
                        {
                            Header = $"{entry.PrimaryName}-{entry.SecondaryName}",
                            Foreground = Brushes.Gray,
                            DiffDecompileEntry = entry,
                            Type = TreeItemType.Function,
                            Cloud = new CloudModel
                            {
                                Initialized = true,
                                IsCloud = false
                            },
                            ToolTip = new TreeToolTipBuilder("", $"Similarity: {entry.Similarity}\t Confidence: {entry.Confidence}", ToolTipTarget.Function).ToString()
                        });
                    }
                    _mainWindow.mainWindowViewModel.TreeItemCaches.Add(father);
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

        public void Window_LocationChanged(object? sender, EventArgs e)
        {

        }

        #endregion

        public MainWindow(string path = "")
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel(this);
            _mainWindowViewModel = this.DataContext as MainWindowViewModel;

            _mainWindow = this;
            keyValuePairs.Add(typeof(IndexPageView), typeof(IndexPageViewModel));
            keyValuePairs.Add(typeof(DiffDecompile), typeof(DiffDecompileViewModel));
            this._setTitle();
            this.Closing += MainWindow_Closing;
            if (!String.IsNullOrEmpty(path))
            {
                _mainWindow.TaskQueues.Enqueue(() =>
                {
                    _mainWindow.AddDiffDecompileToTreeView(path, cache: false);
                });
                parseHistoryWorkflow();
            }
            else
            {
                parseCommandLinesWorkflow();
            }
            _mainWindowViewModel.xxxBuilderService = new Services.xxxBuilder(this);
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