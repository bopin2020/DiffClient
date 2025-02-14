using DiffClient.DataModel;
using DiffClient.UserControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#pragma warning disable 8600
#pragma warning disable 8603

namespace DiffClient.Utility
{
    internal static class QueryObject
    {
        internal static TabItem Execute(MainWindow _mainWindow, string typename)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(Execute)} {typename}", LogStatusLevel.Info);
            return _mainWindow?.rootTab.Items.Cast<TabItem>().Where(x => x.Content.GetType().Name == typename).FirstOrDefault();
        }

        public static TabItem GetIndexPageView(MainWindow _mainWindow)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(GetIndexPageView)} IndexPageView", LogStatusLevel.Info);
            return Execute(_mainWindow, "IndexPageView");
        }

        public static TreeView GetIndexTreeView(MainWindow _mainWindow)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(GetIndexTreeView)} treeview object", LogStatusLevel.Info);
            IndexPageView view = GetIndexPageView(_mainWindow).Content as IndexPageView;
            return view.rootTree;
        }

        public static DiffTreeItem GetIndexTreeViewItemCurrent(MainWindow _mainWindow)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(GetIndexTreeViewItemCurrent)} treeviewitem", LogStatusLevel.Info);
            return GetIndexTreeView(_mainWindow).SelectedItem as DiffTreeItem;
        }

        public static TabItem GetPerformanceView(MainWindow _mainWindow)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(GetPerformanceView)} PerformanceView", LogStatusLevel.Info);
            return Execute(_mainWindow, "PerformanceView");
        }

        public static TreeView GetPerformanceTreeView(MainWindow _mainWindow)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(GetPerformanceTreeView)} treeview object", LogStatusLevel.Info);
            PerformanceView view = GetPerformanceView(_mainWindow).Content as PerformanceView;
            return view.settingTree;
        }

        public static DiffSettingTreeItem GetPerformanceTreeViewItemCurrent(MainWindow _mainWindow)
        {
            MainWindow.SetStatusException($"{nameof(QueryObject)}.{nameof(GetPerformanceTreeViewItemCurrent)} treeviewitem", LogStatusLevel.Info);
            return GetPerformanceTreeView(_mainWindow).SelectedItem as DiffSettingTreeItem;
        }

        public static DiffSetting GetSetting(MainWindow _mainWindow)
        {
            return _mainWindow.mainWindowViewModel.Setting;
        }

        public static bool EnabledPreviewDiffDecompile(MainWindow mainWindow)
        {
            return GetSetting(mainWindow).IsPreviewDiffDecompile;
        }

        public static void SetOSDateInfo(string msg, DiffTreeItem diffTreeItem)
        {
            if (diffTreeItem == null) { return; }
            diffTreeItem.OS = msg.Split(';')[0];
            diffTreeItem.Date = msg.Split(';')[1];
        }
    }

    internal static class DiffClientUtility
    {
        public static IEnumerable<string> OpenDiffDecompileFiles()
        {
            IEnumerable<string> files = new List<string>();

            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = Path.GetTempFileName();
            op.Multiselect = true;
            op.DefaultExt = ".diffdecompile";
            op.Filter = "DiffDecompile (.diffdecompile)|*.diffdecompile";
            op.FilterIndex = 2;
            op.RestoreDirectory = true;
            if (op.ShowDialog() == true)
            {
                return op.FileNames;
            }
            return files;
        }

        public static void ExceptionHandled(Exception ex,string tag, string msg)
        {
            if (Environment.GetEnvironmentVariable("DiffClientLog") == "Debug")
                MessageBox.Show(ex.StackTrace + "\n\n\n" + ex.Message, $"{tag} {msg}", MessageBoxButton.OKCancel);
        }
    }
}
