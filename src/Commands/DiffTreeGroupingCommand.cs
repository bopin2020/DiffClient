using AvalonDock.Controls;
using DiffClient.Pages;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient.Commands
{
    internal enum DiffTreeGroupingCommandRouteEvent
    {
        [Description("Default")]
        Default,
        [Description("OS")]
        OS,
        [Description("OS-Month")]
        OSMonth,
        [Description("Refresh")]
        Refresh,
    }

    internal class DiffTreeGroupingCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public DiffTreeGroupingCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var ctx = parameter as ContextMenuContext;
            MainWindow.SetStatusException($"ProcessRowCommand  param: {ctx.CommandParameter}\n", LogStatusLevel.Info);
            DiffTreeGroupingCommandRouteEvent re = (DiffTreeGroupingCommandRouteEvent)ctx.CommandParameter;

            switch (re)
            {
                case DiffTreeGroupingCommandRouteEvent.Default:
                    {
                        foreach (var item in _mainWindow.mainWindowViewModel.TreeItemCaches)
                        {
                            QueryObject.GetIndexTreeView(_mainWindow).Items.Add(item);
                        }
                    }
                    break;
                case DiffTreeGroupingCommandRouteEvent.OS:
                    {
                        foreach (var item in _mainWindow.mainWindowViewModel.TreeItemCaches.Select(x => x.OS).Distinct())
                        {
                            QueryObject.GetIndexTreeView(_mainWindow).Items.Add(new GroupDiffTreeItem()
                            {
                                Header = item,
                            });
                        }
                    }
                    break;
                case DiffTreeGroupingCommandRouteEvent.OSMonth:
                    {
                        QueryObject.GetIndexPageViewContent(_mainWindow).ViewModel.TreeItemsSource.Clear();
                        foreach (var item in _mainWindow.mainWindowViewModel.TreeItemCaches.Select(x => x.OS).Distinct())
                        {
                            if(item == "") { continue; }
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
                                    ele.FindLogicalAncestor<TreeViewItem>()?.Items.Remove(ele);
                                    subgroup.Items.Add(ele);
                                }
                                subgroup.FindLogicalAncestor<TreeViewItem>()?.Items.Remove(subgroup);
                                group.Children.Add(subgroup);
                            }
                            group.FindLogicalAncestor<TreeViewItem>()?.Items.Remove(group);
                            QueryObject.GetIndexPageViewContent(_mainWindow).ViewModel.TreeItemsSource.Add(group);
                        }
                    }
                    break;
                case DiffTreeGroupingCommandRouteEvent.Refresh:
                    MainWindow.SetStatusException($"refresh: {ctx.CommandParameter}\n", LogStatusLevel.Info);
                    break;
                default:
                    break;
            }
        }
    }
}
