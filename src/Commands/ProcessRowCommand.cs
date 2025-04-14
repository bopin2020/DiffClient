using DiffClient.Pages;
using DiffClient.Utility;
using DiffClient.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient.Commands
{
    internal enum ProcessRowCommandRouteEvent
    {
        [Description("Grouping")]
        Group,
        [Description("Suspending")]
        Suspending,
        [Description("Kill")]
        Kill,
        [Description("Refresh")]
        Refresh,
    }

    internal class ProcessRowCommand : ICommand
    {
       public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public ProcessRowCommand(MainWindow mainWindow)
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
            MainWindow.SetStatusException($"ProcessRowCommand  param: {ctx.CommandParameter}\n",LogStatusLevel.Info);
            string diffresult = "";

            ProcessRowCommandRouteEvent re = (ProcessRowCommandRouteEvent)ctx.CommandParameter;
            switch (re)
            {
                /*
                process grouping rule:
                ppid,  com, rpc and so on

                 */
                case ProcessRowCommandRouteEvent.Group:
                    break;
                case ProcessRowCommandRouteEvent.Suspending:
                    break;
                case ProcessRowCommandRouteEvent.Kill:
                    break;
                case ProcessRowCommandRouteEvent.Refresh:
                    var ProcessPageModel =  ctx.ViewAndViewModelContext as ProcessListPageModel;
                    if(ProcessPageModel == null)
                    {
                        break;
                    }
                    ProcessPageModel.Refresh(Help.GetProcesses());
                    break;
                default:
                    break;
            }
        }
    }

}
