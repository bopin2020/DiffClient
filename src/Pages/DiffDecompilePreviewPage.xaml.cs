﻿using DiffClient.DataModel;
using DiffClient.Utility;
using DiffClient.Windows;
using DiffDecompile.Core;
using DiffPlex.WindowsForms.Controls;
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

#pragma warning disable

namespace DiffClient.Pages
{
    /// <summary>
    /// DiffDecompilePreviewPage.xaml 的交互逻辑
    /// </summary>
    public partial class DiffDecompilePreviewPage : Page
    {
        #region Private Members

        private bool backendInit = false;
        private bool dataformInit = false;
        private DiffDecompilePreviewPageModel model;

        private void LeftCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(DiffView.OldText);
            MessageBox.Show($"Copy Size: {DiffView.OldText.Length}");
        }

        private void LeftEdit_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditDiffCodeWindow(DecompileSourceType.IDAPro, DataType.Decompilation, DiffView.OldText, Callback, true);
            window.ShowDialog();
        }

        private void RightCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(DiffView.NewText);
            MessageBox.Show($"Copy Size: {DiffView.NewText.Length}");
        }

        private void RightEdit_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditDiffCodeWindow(DecompileSourceType.IDAPro, DataType.Decompilation, DiffView.NewText,Callback,false);
            window.ShowDialog();
        }
        private void Callback(string code, bool isleft = false)
        {
            if (isleft)
            {
                DiffView.OldText = code;
            }
            else { DiffView.NewText = code; }

            DiffView.Refresh();
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
                    if (String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content))
                    {
                        MainWindow.SetStatusException("search content is null", LogStatusLevel.Warning);
                        return;
                    }

                    int index = SearchRange.IndexOfLine(content);
                    if (index == -1 || index == 0)
                    {
                        MainWindow.SetStatusException($"search {content} is not found", LogStatusLevel.Warning);
                        return;
                    }
                    // todo 
                    DiffView.GoTo(index);
                    MainWindow.SetStatusException($"jump to {index}", LogStatusLevel.Info);
                }
                catch (Exception)
                {
                    MainWindow.SetStatusException($"TextBox input is invalid");
                }
            }
        }

        private void BackendSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (backendInit)
            {
                DiffView.OldText = "test";
                DiffView.NewText = "new test";
            }
            backendInit = true;
        }

        private void DataForms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataformInit)
            {
                DiffView.OldText = "test";
                DiffView.NewText = "new test";
            }
            dataformInit = true;
        }
        
        #endregion

        #region Public Members

        public string SearchRange { get; set; }

        #endregion

        public DiffDecompilePreviewPage(MainWindow mainWindow,DiffDecompileArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            InitializeComponent();
            this.DataContext = model = new DiffDecompilePreviewPageModel(mainWindow, this);
            MainWindow.SetStatusException($"{nameof(DiffDecompilePreviewPage)}.SetDiffModel", LogStatusLevel.Info);
            DiffView.OldText = item.Primary;
            DiffView.NewText = item.Secondary;
            SearchRange = item.Secondary;
            LoadData();
        }
    }
}
