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
using System.Windows.Shapes;
using DiffClient.Pages;

namespace DiffClient.Windows
{
    /// <summary>
    /// HistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private SettingManager SettingManager;
        public HistoryWindow()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {

        }

        private void historyFrame_Initialized(object sender, EventArgs e)
        {
            if (SettingManager == null)
            {
                SettingManager = new SettingManager(null);
                SettingManager.InitOrRegisterSetting();
            }
            historyFrame.NavigationService.Navigate(new HistoryFilterPage(this,SettingManager.GetHistories().Select(x => new HistoryEntry() { FullName = x })));
        }
    }
}
