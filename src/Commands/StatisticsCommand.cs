using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffClient.Pages;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DiffClient.DataModel;

namespace DiffClient.Commands
{
    internal enum StatisticsCommandRouteEvent
    {
        [Description("Diff View")]
        DiffView,
        [Description("Tag")]
        Tag,
    }

    internal class StatisticsCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public StatisticsCommand(MainWindow mainWindow)
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
            if (ctx == null || ctx.DataGrid == null)
            {
                return;
            }

            StatisticsCommandRouteEvent re = (StatisticsCommandRouteEvent)ctx.CommandParameter;
            var jpm = ctx.ViewAndViewModelContext as StatisticsPageModel;
            if (jpm == null)
            {
                return;
            }
            var currow = ctx.DataGrid.SelectedItem as DiffDecompileItem;
            if (currow == null)
            {
                return;
            }

            switch (re)
            {
                case StatisticsCommandRouteEvent.Tag:
                    var selectedItem = ctx.DataGrid.SelectedItem;
                    if (selectedItem != null)
                    {
                        DataGridRow row = ctx.DataGrid?.ItemContainerGenerator.ContainerFromItem(selectedItem) as DataGridRow;
                        if (row != null)
                        {
                            row.Background = Brushes.AliceBlue;
                        }
                    }
                    break;
                case StatisticsCommandRouteEvent.DiffView:
                    ctx.indexFrame.NavigationService.Navigate(
                             new DiffDecompilePreviewPage(_mainWindow, new DiffDecompileArgs()
                             {
                                 Title = $"{currow.PrimaryName}-{currow.SecondaryName}",
                                 Primary = currow.PrimaryData,
                                 Secondary = currow.SecondaryData
                             }));
                    break;
                default:
                    break;
            }

            ctx.DataGrid.Items.Refresh();
        }
    }

}
