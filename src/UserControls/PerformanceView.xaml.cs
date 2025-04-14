using DiffClient.Commands;
using DiffClient.DataModel;
using DiffClient.Pages;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static DiffClient.Utility.Help;

#pragma warning disable

namespace DiffClient.UserControls
{
    internal sealed class NavigationServiceManager
    {
        private Dictionary<DiffSettingTreeItem,Page> _cache = new Dictionary<DiffSettingTreeItem,Page>();

        private DiffSettingTreeItem _cur;
        private DiffSettingTreeItem _last = null;

        private Page _curpage;
        private Page _lastpage = null;

        private Frame _root;

        public NavigationServiceManager(Frame frame)
        {
            if(_root == null)
            {
                _root = frame;
            }
        }

        public void OpenStateMachine(DiffSettingTreeItem item, Page page)
        {
            _cur = item;
            _curpage = page;
        }

        public void CloseStateMachine(DiffSettingTreeItem item, Page page)
        {

        }

        public void ProxyInvoke()
        {
            if (_cache.ContainsKey(_cur))
            {
                if (_cache.TryGetValue(_cur, out var page))
                    _root.NavigationService.Navigate(page);
                return;
            }
            _cache.Add(_cur, _curpage);
            _root.NavigationService.Navigate(_curpage);
        }
    }

    /// <summary>
    /// PerformanceView.xaml 的交互逻辑
    /// </summary>
    public partial class PerformanceView : UserControl
    {
        #region Private Members

        private MainWindow _mainWindow;
        private NavigationServiceManager _navigationServiceManager;

        private void settingTree_Initialized(object sender, EventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(settingTree_Initialized)} ", LogStatusLevel.Info);
            settingTree.Items.Add(new DiffSettingTreeItem() { ModuleName = "Cloud Service", Header = "Cloud Service" });
            settingTree.Items.Add(new DiffSettingTreeItem() { ModuleName = "GenericProperty", Header = "Log" });
            settingTree.Items.Add(new DiffSettingTreeItem() { ModuleName = "Preview", Header = "EnabledPreviewDiffDecompile" });
            settingTree.Items.Add(new DiffSettingTreeItem() { ModuleName = "History", Header = "History" });
            settingTree.Items.Add(new DiffSettingTreeItem() { ModuleName = "Misc", Header = "Process" });
        }

        private void settingTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void settingTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(settingTree_MouseDoubleClick)} ", LogStatusLevel.Info);
            DiffSettingTreeItem diffSettingTreeItem = settingTree.SelectedItem as DiffSettingTreeItem;

            if (diffSettingTreeItem.Header.ToString() == "Cloud Service")
            {
                // todo 
                //_navigationServiceManager.OpenStateMachine(diffSettingTreeItem, new SettingCloudPage(_mainWindow));
                //_navigationServiceManager.ProxyInvoke();
                settingFrame.NavigationService.Navigate(new SettingCloudPage(_mainWindow));
            }
            else if (diffSettingTreeItem.Header.ToString() == "Log")
            {
                settingFrame.NavigationService.Navigate(new GenericLogPage(_mainWindow));
            }
            else if (diffSettingTreeItem.Header.ToString() == "EnabledPreviewDiffDecompile")
            {
                settingFrame.NavigationService.Navigate(new GenericLogPage(_mainWindow));
            }
            else if (diffSettingTreeItem.Header.ToString() == "History")
            {
                settingFrame.NavigationService.Navigate(new HistoryPage(_mainWindow));
            }
            else if (diffSettingTreeItem.Header.ToString() == "Process")
            {
                settingFrame.NavigationService.Navigate(new ProcessListPage(_mainWindow, Help.GetProcesses()));
            }
        }

        private void settingsplitter_Initialized(object sender, EventArgs e)
        {

        }

        private void settingGraph_Initialized(object sender, EventArgs e)
        {

        }

        private void Store_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.mainWindowViewModel.SaveSetting();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
        
        #endregion

        #region Public Members



        #endregion

        public PerformanceView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            _navigationServiceManager = new NavigationServiceManager(settingFrame);
        }
    }
}
