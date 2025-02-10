using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DiffClient
{
    /// <summary>
    /// DiffDecompile.xaml 的交互逻辑
    /// </summary>
    public partial class DiffDecompile : UserControl
    {
        private string searchRange { get; set; }
        public DiffDecompile(DiffDecompileArgs item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }
            InitializeComponent();
            MainWindow.SetStatusException($"{nameof(DiffDecompile)}.SetDiffModel", LogStatusLevel.Info);
            DiffView.OldText = item.Primary;
            DiffView.NewText = item.Secondary;
            searchRange = item.Secondary;
            LoadData();
        }

        private void LoadData()
        {
            var now = DateTime.Now;
            var isDark = now.Hour < 6 || now.Hour >= 18;
            DiffView.Foreground = new SolidColorBrush(isDark ? Color.FromRgb(240, 240, 240) : Color.FromRgb(32, 32, 32));
            DiffView.SetHeaderAsOldToNew();
        }

        private void DiffButton_Click(object sender, RoutedEventArgs e)
        {
            if (DiffView.IsInlineViewMode)
            {
                DiffView.ShowSideBySide();
                return;
            }

            DiffView.ShowInline();
        }
        private void FutherActionsButton_Click(object sender, RoutedEventArgs e)
        {
            DiffView.OpenViewModeContextMenu();
        }

        private void gotoBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int off = 0;
                try
                {
                    off = Convert.ToInt32(gotoBox.Text);
                    DiffView.GoTo(off);
                }
                catch (Exception)
                {
                    MainWindow.SetStatusException($"TextBox input is invalid", LogStatusLevel.Warning);
                }
            }
        }

        private void GotoPrev(object sender, RoutedEventArgs e)
        {
            DiffView.GoTo(DiffView.PreviousDiff());
        }

        private void GotoNext(object sender, RoutedEventArgs e)
        {
            DiffView.GoTo(DiffView.NextDiff());
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    string content = searchBox.Text;
                    if(String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content))
                    {
                        MainWindow.SetStatusException("search content is null", LogStatusLevel.Warning);
                        return;
                    }

                    int index = searchRange.IndexOfLine(content);
                    if (index == -1 || index == 0)
                    {
                        MainWindow.SetStatusException($"search {content} is not found", LogStatusLevel.Warning);
                        return;
                    }

                    DiffView.GoTo(index);
                    MainWindow.SetStatusException($"", LogStatusLevel.Info);
                }
                catch (Exception)
                {
                    MainWindow.SetStatusException($"TextBox input is invalid");
                }
            }
        }
    }
}
