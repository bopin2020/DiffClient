using DiffClient.Commands;
using DiffClient.DataModel;
using DiffClient.Pages;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
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

namespace DiffClient.UserControls
{
    /// <summary>
    /// PerformanceView.xaml 的交互逻辑
    /// </summary>
    public partial class PerformanceView : UserControl
    {
        private MainWindow _mainWindow;
        public PerformanceView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private void settingTree_Initialized(object sender, EventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(settingTree_Initialized)} ", LogStatusLevel.Info);
            settingTree.Items.Add(new DiffSettingTreeItem(){ ModuleName = "Cloud Service", Header = "Cloud Service" });
            settingTree.Items.Add(new DiffSettingTreeItem(){ ModuleName = "GenericProperty", Header = "Log" });
            settingTree.Items.Add(new DiffSettingTreeItem(){ ModuleName = "Preview", Header = "EnabledPreviewDiffDecompile" });
            settingTree.Items.Add(new DiffSettingTreeItem(){ ModuleName = "History", Header = "History" });
        }

        private void settingTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void settingTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(settingTree_MouseDoubleClick)} ", LogStatusLevel.Info);
            DiffSettingTreeItem diffSettingTreeItem = settingTree.SelectedItem as DiffSettingTreeItem;

            if(diffSettingTreeItem.Header.ToString() == "Cloud Service")
            {
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
    }
}
