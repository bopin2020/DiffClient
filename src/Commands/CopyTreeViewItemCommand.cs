using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffClient.Utility;
using System.Windows.Input;
using System.Windows.Forms;

namespace DiffClient.Commands
{
    internal enum CopyTreeViewItemRouteEvent
    {
        Copy,
        CopyFunctions
    }

    internal class CopyTreeViewItemCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public CopyTreeViewItemCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MainWindow.SetStatusException($"{nameof(HighlightTreeViewItemCommand)} invoked", LogStatusLevel.Warning);
            DiffTreeItem tmp = QueryObject.GetIndexTreeViewItemCurrent(_mainWindow);
            CopyTreeViewItemRouteEvent ev = (CopyTreeViewItemRouteEvent)parameter;
            switch (ev)
            {
                case CopyTreeViewItemRouteEvent.Copy:
                    string result = tmp.Header.ToString();
                    MessageBox.Show(result);
                    Clipboard.SetDataObject(result);
                    break;
                case CopyTreeViewItemRouteEvent.CopyFunctions:
                    string result2 = String.Join('\n', tmp.Children.Select(x => x.Header.ToString()).ToArray());
                    MessageBox.Show(result2);
                    Clipboard.SetDataObject(result2);
                    break;
                default:
                    break;
            }
        }
    }
}
