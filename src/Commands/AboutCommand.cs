using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffClient.Utility;
using DiffClient.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#pragma warning disable

namespace DiffClient.Commands
{
    internal class AboutCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public AboutCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MainWindow.SetStatusException($"{nameof(AboutCommand)} invoked", LogStatusLevel.Info);
            var vw = new AboutWindow(_mainWindow);
            vw.DataContext = new AboutWindowViewModel(vw);
            vw.ShowDialog();
        }
    }

}
