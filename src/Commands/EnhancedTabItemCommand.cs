using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

#pragma warning disable

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

            var item = parameter as TabItem;
            if (item == null) { 
                throw new ArgumentNullException(nameof(parameter));
            }
            if(MainWindowViewModel.TabControlCached.Values.Contains(parameter))
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
    }
}
