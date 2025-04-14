using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient.Commands
{
    internal enum ContextMenuRouteEvent
    {
        [Description("Copy C")]
        CopyC,
        [Description("Clear content")]
        Clear,
        [Description("Undo")]
        Undo
    }

    internal class ContextMenuCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public ContextMenuCommand(MainWindow mainWindow)
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

            ContextMenuRouteEvent re = (ContextMenuRouteEvent)ctx.CommandParameter;

            switch (re)
            {
                case ContextMenuRouteEvent.CopyC:
                    break;
                case ContextMenuRouteEvent.Clear:
                    if(ctx.RichTextBox != null)
                    {
                        ctx.RichTextBox.Document.Blocks.Clear();
                    }
                    break;
                case ContextMenuRouteEvent.Undo:
                    break;
                default:
                    break;
            }
        }
    }

}
