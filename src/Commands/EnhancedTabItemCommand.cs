using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient.Commands
{
    internal enum EnhancedTabItemCommandRouteEvent
    {
        [Description("Detach")]
        Detach,
        [Description("Highlight")]
        Highlight,
        [Description("Disable Close")]
        Disabled,
    }

    internal class EnhancedTabItemCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        private bool _enabled = true;
        public EnhancedTabItemCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return _enabled;
        }

        public void Execute(object? parameter)
        {
            var ctx = parameter as ContextMenuContext;
            if (ctx != null)
            {
                MainWindow.SetStatusException($"{parameter.GetType().Name} invoked", LogStatusLevel.Info);
                EnhancedTabItemCommandRouteEvent re = (EnhancedTabItemCommandRouteEvent)ctx.CommandParameter;
                switch (re)
                {
                    case EnhancedTabItemCommandRouteEvent.Detach:
                        {
                            RemoveIt(parameter);
                        }
                        break;
                    case EnhancedTabItemCommandRouteEvent.Highlight:
                        break;
                    case EnhancedTabItemCommandRouteEvent.Disabled:
                        _enabled = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                RemoveIt(parameter);
            }
        }

        public void RemoveIt(object parameter)
        {
            void do_(object? parameter)
            {
                var item = parameter as TabItem;
                if (item == null)
                {
                    return;
                }
                if (MainWindowViewModel.TabControlCached.Values.Contains(parameter))
                {
                    var tmp = MainWindowViewModel.TabControlCached.Where(x => x.Value == parameter).Select(x => x.Key).FirstOrDefault();
                    if (tmp != "root")
                    {
                        MainWindowViewModel.TabControlCached.Remove(tmp);
                    }
                }

                _mainWindow.rootTab?.Items.Remove(parameter);
                if (_mainWindow.rootTab?.SelectedIndex != 0)
                    _mainWindow.rootTab.SelectedIndex = _mainWindow.rootTab.SelectedIndex - 1;

            }
            
            var ctx = parameter as ContextMenuContext;
            if (ctx != null)
            {
                do_(ctx.tabItem);
            }
            else
            {
                do_(parameter);
            }
        }
    }
}
