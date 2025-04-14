using DiffClient.UserControls;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#pragma warning disable

namespace DiffClient.Commands
{
    internal class DeleteTreeViewItemCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public DeleteTreeViewItemCommand(MainWindow mainWindow)
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
            var tmp = QueryObject.GetIndexTreeViewItemCurrent(_mainWindow);
            QueryObject.GetIndexTreeView(_mainWindow).Items.Remove(tmp);
        }
    }
}
