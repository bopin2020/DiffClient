using DiffClient.Windows;
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
    internal enum DataGridColumnRouteEvent
    {
        [Description("Size all columns to fit")]
        FitAll,
        [Description("Hide")]
        Hide,
        [Description("Show")]
        Show,
        [Description("Choose columns")]
        Choose,
        [Description("Grouping")]
        Group,
    }

    internal class DataGridColumnCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public DataGridColumnCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }
        /// <summary>
        /// todo
        /// DataGrid   Group
        /// https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-group-sort-and-filter-data-in-the-datagrid-control?view=netframeworkdesktop-4.8
        /// </summary>
        /// <param name="dg"></param>
        public void RefitColumnWidth(DataGrid dg)
        {
            foreach (var item in dg.Columns)
            {
                //item.Width = 100;
            }
        }

        public void Execute(object? parameter)
        {
            var ctx = parameter as ContextMenuContext;
            if(ctx.DataGrid == null)
            {
                return;
            }

            DataGridColumnRouteEvent re = (DataGridColumnRouteEvent)ctx.CommandParameter;
            switch (re)
            {
                case DataGridColumnRouteEvent.FitAll:
                    RefitColumnWidth(ctx.DataGrid);
                    break;
                case DataGridColumnRouteEvent.Hide:
                    break;
                case DataGridColumnRouteEvent.Show:
                    break;
                case DataGridColumnRouteEvent.Choose:
                    var window = new DefaultDialogWindow(ctx.DataGrid);
                    window.ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }

}
