using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient.Commands
{
    internal class HistoryCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public HistoryCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MainWindow.SetStatusException($"{nameof(HistoryCommand)} {parameter.ToString()}", LogStatusLevel.Warning);
            _mainWindow.AddDiffDecompileToTreeView(parameter.ToString());
        }
    }
}
