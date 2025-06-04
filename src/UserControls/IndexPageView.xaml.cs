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

#pragma warning disable

namespace DiffClient.UserControls
{
    /// <summary>
    /// IndexPageView.xaml 的交互逻辑
    /// </summary>
    public partial class IndexPageView : UserControl
    {
        #region Private Members

        private MainWindow _mainWindow;

        private void _dispatchTreeItemEvent(DiffTreeItem item)
        {
            switch (item.Type)
            {
                case TreeItemType.None:
                    MainWindow.SetStatusException($"{item.Name} type is None",LogStatusLevel.Warning);
                    break;
                case TreeItemType.Local:
                    MainWindow.SetStatusException($"{item.Name} Local", LogStatusLevel.Info);
                    break;
                case TreeItemType.Cloud:
                    MainWindow.SetStatusException($"{item.Name} Cloud", LogStatusLevel.Info);
                    break;
                case TreeItemType.Group:
                    MainWindow.SetStatusException($"{item.Name} Group", LogStatusLevel.Info);
                    break;
                case TreeItemType.TreeView:
                    MainWindow.SetStatusException($"{item.Name} TreeView", LogStatusLevel.Info);
                    break;
                case TreeItemType.Function:
                    MainWindow.SetStatusException($"{item.Name} Function", LogStatusLevel.Info);
                    break;
                case TreeItemType.LocalDirectory:
                    MainWindow.SetStatusException($"{item.Name} LocalDirectory", LogStatusLevel.Info);
                    try
                    {
                        if (Directory.GetFiles(item.FullPath, "*.diffdecompile").Count() > 0)
                        {
                            if (!item.IsLocalInit)
                            {
                                foreach (var file in Directory.GetFiles(item.FullPath, "*.diffdecompile"))
                                {
                                    item.AddProxy(new DiffTreeItem()
                                    {
                                        FullPath = file,
                                        Header = new FileInfo(file).Name,
                                        Type = TreeItemType.LocalTreeItem,
                                        Foreground = Brushes.Gray,
                                    });
                                }
                                item.IsLocalInit = true;
                            }
                        }
                        else
                        {
                            if (!item.IsLocalInit)
                            {
                                foreach (var dir in Directory.GetDirectories(item.FullPath))
                                {
                                    item.AddProxy(new DiffTreeItem()
                                    {
                                        FullPath = dir,
                                        Header = new DirectoryInfo(dir).Name,
                                        Type = TreeItemType.LocalDirectory,
                                        Foreground = Brushes.Gray,

                                    });
                                }
                                item.IsLocalInit = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MainWindow.SetStatusException($"{ex.Message}", LogStatusLevel.Error);
                    }
                    break;
                case TreeItemType.LocalRoot:
                    MainWindow.SetStatusException($"{item.Name} LocalRoot", LogStatusLevel.Info);
                    item.IsExpanded = true;
                    break;
                case TreeItemType.LocalTreeItem:
                    MainWindow.SetStatusException($"{item.Name} LocalTreeItem", LogStatusLevel.Info);
                    _mainWindow.AddDiffDecompileToTreeView(item.FullPath, true,item);
                    indexFrame.NavigationService.Navigate(new StatisticsPage(_mainWindow, indexFrame, item.Entries.Select(x => new DiffDecompileItem()
                    {
                        Id = x.Id,
                        Similarity = x.Similarity,
                        Confidence = x.Confidence,
                        PrimaryName = x.PrimaryName,
                        PrimaryAddress = x.PrimaryAddress,
                        SecondaryName = x.SecondaryName,
                        SecondaryAddress = x.SecondaryAddress,
                        PrimaryData = x.PrimaryDecompileCode.Value,
                        SecondaryData = x.SecondaryDecompileCode.Value,
                    }).ToList()));
                    item.IsExpanded = true;
                    break;
                default:
                    break;
            }
        }

        private void rootTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tree = (TreeView)sender;

            if (tree != null)
            {
                try
                {
                    DiffTreeItem item = (DiffTreeItem)tree.SelectedItem;
                    if (item == null) { return; }
                    MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_SelectedItemChanged)} {item.Header}", LogStatusLevel.Info);
                    _dispatchTreeItemEvent(item);

                    item.Foreground = Brushes.Black;

                    if (item.IsDiffModuleUnit)
                    {
                        if (item.Cloud.IsCloud)
                        {
                            MainWindow.SetStatusException($"AccessCloud diffdecompile file {item.FullPath}", LogStatusLevel.Info);
                            _mainWindow.AddDiffDecompileToTreeViewFromUrl(item, item.FullPath, true);
                        }
                        indexFrame.NavigationService.Navigate(new StatisticsPage(_mainWindow,indexFrame,item.Entries.Select(x => new DiffDecompileItem()
                        {
                            Id = x.Id,
                            Similarity = x.Similarity,
                            Confidence = x.Confidence,
                            PrimaryName = x.PrimaryName,
                            PrimaryAddress = x.PrimaryAddress,
                            SecondaryName = x.SecondaryName,
                            SecondaryAddress = x.SecondaryAddress,
                            PrimaryData = x.PrimaryDecompileCode.Value,
                            SecondaryData = x.SecondaryDecompileCode.Value,
                        }).ToList()));
                        item.IsExpanded = true;
                    }
                    else
                    {
                        if (QueryObject.EnabledPreviewDiffDecompile(_mainWindow))
                        {
                            indexFrame.NavigationService.Navigate(
                                 new DiffDecompilePreviewPage(_mainWindow, new DiffDecompileArgs()
                                 {
                                     Title = item.Header.ToString(),
                                     Primary = item.DiffDecompileEntry.PrimaryDecompileCode.Value,
                                     Secondary = item.DiffDecompileEntry.SecondaryDecompileCode.Value,

                                 })
                            );
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void rootTree_Initialized(object sender, EventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_Initialized)} ", LogStatusLevel.Info);
            ContextMenu cm = new DiffTreeInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
            }).Register();

            cm.Items.Add(new Separator());
            cm.Items.Add(new MenuItem() { Header = "Highlight", Command = new HighlightTreeViewItemCommand(_mainWindow) });
            cm.Items.Add(new MenuItem() { Header = "Delete", Command = new DeleteTreeViewItemCommand(_mainWindow) });
            cm.Items.Add(new Separator());
            cm.Items.Add(new MenuItem() { Header = "Copy", Command = new CopyTreeViewItemCommand(_mainWindow),CommandParameter = CopyTreeViewItemRouteEvent.Copy });
            cm.Items.Add(new MenuItem() { Header = "Copy Functions", Command = new CopyTreeViewItemCommand(_mainWindow), CommandParameter = CopyTreeViewItemRouteEvent.CopyFunctions });
            rootTree.ContextMenu = cm;
        }

        private void rootTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.SetStatusException($"{nameof(IndexPageView)}.{nameof(rootTree_MouseDoubleClick)} ", LogStatusLevel.Info);
            TreeView tree = (TreeView)sender;
            if (tree != null)
            {
                DiffTreeItem item = tree.SelectedItem as DiffTreeItem;
                if (item == null)
                {
                    MainWindow.SetStatusException($"treeview item select type is not DiffTreeItem", LogStatusLevel.Info);
                    return;
                }

                if (item.DiffDecompileEntry != null)
                {
                    _mainWindow.rootTab?.Items.Add(new DiffDecompileTabItem(_mainWindow, new DiffDecompileArgs()
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
                    _mainWindow.AddDiffDecompileToTreeViewFromUrl(item, item.FullPath, true);
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
                if (Directory.Exists(item))
                {
                    _mainWindow.AddDirectoryToTreeView(item);
                }
                else
                    _mainWindow.AddDiffDecompileToTreeView(item, true);
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

        #endregion

        #region Public Members

        internal IndexPageViewModel ViewModel { get; set; }

        #endregion

        public IndexPageView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }
    }
}
