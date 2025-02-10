using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiffClient.Commands
{
    internal class EnhancedTabItemCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public EnhancedTabItemCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MainWindow.SetStatusException($"{parameter.GetType().Name} invoked", LogStatusLevel.Info);
            _mainWindow.rootTab?.Items.Remove(parameter);
            if (_mainWindow.rootTab?.SelectedIndex != 0)
                _mainWindow.rootTab.SelectedIndex = _mainWindow.rootTab.SelectedIndex - 1;
        }
    }
}
