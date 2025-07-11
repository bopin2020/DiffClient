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
using System.IO;
using System.Diagnostics;

namespace DiffClient.Commands
{
    internal enum StatisticsCommandRouteEvent
    {
        [Description("Diff View")]
        DiffView,
        [Description("Diff Tab")]
        DiffTab,
        [Description("Diff With VS Code")]
        DiffVSCode,
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

        /// <summary>
        /// xxx
        /// xxx(int* p,xxx)
        /// xxx::xxx::xxx
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static string FilterFuncName(string fullname)
        {
            if (String.IsNullOrEmpty(fullname))
            {
                throw new ArgumentNullException(fullname);
            }

            if(!fullname.Contains("*") && !fullname.Contains("::"))
            {
                return fullname;
            }
            string pure_methodname = "";
            int index = fullname.IndexOf('(');
            if(index > 0)
            {
                pure_methodname = fullname.Substring(0, index);
            }

            if (pure_methodname.Contains("::"))
            {
                pure_methodname = pure_methodname.Replace("::", "__");
            }

            return pure_methodname;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var ctx = parameter as ContextMenuContext;
            if (ctx == null || ctx?.DataGrid == null)
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
                        DataGridRow? row = ctx.DataGrid?.ItemContainerGenerator.ContainerFromItem(selectedItem) as DataGridRow;
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
                case StatisticsCommandRouteEvent.DiffTab:
                    _mainWindow.rootTab?.Items.Add(new DiffDecompileTabItem(_mainWindow, new DiffDecompileArgs()
                    {
                        Title = $"{currow.PrimaryName}-{currow.SecondaryName}",
                        Primary = currow.PrimaryData,
                        Secondary = currow.SecondaryData
                    }));
                    break;
                case StatisticsCommandRouteEvent.DiffVSCode:
                    string oldname = Path.Combine(_mainWindow.mainWindowViewModel.DiffClientWorkDir.FullName, FilterFuncName(currow.PrimaryName) + "-old.c");
                    string newname = Path.Combine(_mainWindow.mainWindowViewModel.DiffClientWorkDir.FullName, FilterFuncName(currow.SecondaryName) + "-new.c");
                    try
                    {
                        File.WriteAllText(oldname, currow.PrimaryData);
                        File.WriteAllText(newname, currow.SecondaryData);
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = "cmd";
                        psi.Arguments = $"/c code -d {oldname} {newname}";
                        MainWindow.SetStatusException($"Start process command arguments: {psi.Arguments}", LogStatusLevel.Info);
                        Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        MainWindow.SetStatusException(ex.Message,LogStatusLevel.Error);
                        if(!String.IsNullOrEmpty(ex.StackTrace))
                            MainWindow.SetStatusException(ex.StackTrace,LogStatusLevel.Error);
                        File.Delete(oldname);
                        File.Delete(newname);
                    }
                    break;
                default:
                    break;
            }

            ctx.DataGrid.Items.Refresh();
        }
    }

}
