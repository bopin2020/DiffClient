using DiffClient.Commands;
using DiffClient.DataModel;
using DiffClient.Pages;
using DiffClient.Utility;
using DiffEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

#pragma warning disable 0253
#pragma warning disable 8600
#pragma warning disable 8601

namespace DiffClient.UserControls
{
    /// <summary>
    /// IndexPageView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexPageView : UserControl
    {
        MainWindow _mainWindow;
        public IndexPageView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private void rootTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = (TreeView)sender;
            if(tree != null)
            {
                try
                {
                    DiffTreeItem item = (DiffTreeItem)tree.SelectedItem;
                    if (item == null) { return; }
                    MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_SelectedItemChanged)} {item.Header}", LogStatusLevel.Info);
                    item.Foreground = Brushes.Black;
                    if (item.IsDiffModuleUnit)
                    {
                        if (item.Cloud.IsCloud)
                        {
                            MainWindow.SetStatusException($"AccessCloud diffdecompile file {item.FullPath}", LogStatusLevel.Info);
                            _mainWindow.AddDiffDecompileToTreeViewFromUrl(item, item.FullPath);
                        }
                        indexFrame.NavigationService.Navigate(new StatisticsPage(_mainWindow, item.Entries.Select(x => new DiffDecompileItem()
                        {
                            Id = x.Id,
                            Similarity = x.Similarity,
                            Confidence = x.Confidence,
                            PrimaryName = x.PrimaryName,
                            PrimaryAddress = x.PrimaryAddress,
                            SecondaryName = x.SecondaryName,
                            SecondaryAddress = x.SecondaryAddress,
                        }).ToList()));
                        item.IsExpanded = true;
                    }
                    else
                    {
                        if (QueryObject.EnabledPreviewDiffDecompile(_mainWindow))
                        {
                            indexFrame.NavigationService.Navigate(new DiffDecompilePreviewPage(_mainWindow, new DiffDecompileArgs()
                            {
                                Title = item.Header.ToString(),
                                Primary = item.DiffDecompileEntry.PrimaryDecompileCode.Value,
                                Secondary = item.DiffDecompileEntry.SecondaryDecompileCode.Value,
                            }));
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        class NatigateProxy
        {

        }

        private void rootTree_Initialized(object sender, EventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_Initialized)} ", LogStatusLevel.Info);
            ContextMenu cm = new ContextMenu();
            cm.Items.Add(new MenuItem() { Header = "Highlight", Command = new HighlightTreeViewItemCommand(_mainWindow) });
            cm.Items.Add(new MenuItem() { Header = "Delete", Command = new DeleteTreeViewItemCommand(_mainWindow) });
            rootTree.ContextMenu = cm;
        }

        private void rootTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_MouseDoubleClick)} ", LogStatusLevel.Info);
            TreeView tree = (TreeView)sender;
            if (tree != null)
            {
                DiffTreeItem item = tree.SelectedItem as DiffTreeItem;
                if(item == null)
                {
                    MainWindow.SetStatusException($"treeview item select type is not DiffTreeItem", LogStatusLevel.Info);
                    return;
                }

                if(item.DiffDecompileEntry != null)
                {
                    _mainWindow.rootTab?.Items.Add(new DiffDecompileTabItem(_mainWindow,new DiffDecompileArgs()
                    {
                        Title = item.Header.ToString(),
                        Primary = item.DiffDecompileEntry.PrimaryDecompileCode.Value,
                        Secondary = item.DiffDecompileEntry.SecondaryDecompileCode.Value,
                    }));
                    return;
                }
                // todo
                if (item.Cloud.IsCloud)
                {
                    MainWindow.SetStatusException($"AccessCloud diffdecompile file {item.FullPath}", LogStatusLevel.Info);
                    _mainWindow.AddDiffDecompileToTreeViewFromUrl(item,item.FullPath);
                }
            }
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(indexFrame_Initialized)} ", LogStatusLevel.Info);
            indexFrame.NavigationService.Navigate(new IndexInitPage(_mainWindow));
        }

        private void rootTree_DragEnter(object sender, DragEventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_DragEnter)} ", LogStatusLevel.Info);
            TreeView root = sender as TreeView;
            foreach (string item in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                _mainWindow.AddDiffDecompileToTreeView(item,true);
            }
        }

        private void indexGraph_Initialized(object sender, EventArgs e)
        {
            // todo  load page to ..
            //indexGraph.Children.Add(new IndexInit());
        }

        private void indexsplitter_Initialized(object sender, EventArgs e)
        {
        }

        private void indexFrame_Initialized(object sender, EventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(indexFrame_Initialized)} ", LogStatusLevel.Info);
            indexFrame.NavigationService.Navigate(new IndexInitPage(_mainWindow));
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            QueryObject.GetIndexTreeView(_mainWindow).Items.Clear();
            switch (rb.Content)
            {
                case "Default":
                    foreach (var item in _mainWindow.mainWindowViewModel.TreeItemCaches)
                    {
                        QueryObject.GetIndexTreeView(_mainWindow).Items.Add(item);
                    }
                    break;
                case "OS":
                    foreach (var item in _mainWindow.mainWindowViewModel.TreeItemCaches.Select(x => x.OS).Distinct())
                    {
                        QueryObject.GetIndexTreeView(_mainWindow).Items.Add(new GroupDiffTreeItem()
                        {
                            Header = item,
                        });
                    }
                    break;
                case "OS-Month":
                    foreach (var item in _mainWindow.mainWindowViewModel.TreeItemCaches.Select(x => x.OS).Distinct())
                    {
                        var group = new GroupDiffTreeItem()
                        {
                            Header = item,
                            IsExpanded = true,
                        };
                        foreach (var mon in _mainWindow.mainWindowViewModel.TreeItemCaches.Where(x => x.OS == group.Header).Select(x => x.Date).Distinct())
                        {
                            var subgroup = new GroupDiffTreeItem()
                            {
                                Header = mon,
                                IsExpanded = true
                            };

                            foreach (var ele in _mainWindow.mainWindowViewModel.TreeItemCaches.Where(x => x.OS == group.Header && x.Date == subgroup.Header).Distinct())
                            {
                                subgroup.Items.Add(ele);
                            }
                            group.Items.Add(subgroup);
                        }
                        QueryObject.GetIndexTreeView(_mainWindow).Items.Add(group);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
