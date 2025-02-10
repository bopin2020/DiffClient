using DiffClient.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#pragma warning disable 1591
#pragma warning disable 8600

namespace DiffClient.Commands
{
    internal class ExitCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public ExitCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            ExitFromClick(false);
            Environment.Exit(0);
        }

        public void ExitFromClick(bool click)
        {
            MainWindow.SetStatusException($"{nameof(ExitCommand)} invoked", LogStatusLevel.Warning);
            if (MessageBox.Show("是否保存设置", "setting", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _mainWindow.mainWindowViewModel.SaveSetting();
                _mainWindow.mainWindowViewModel.GlobalLogStream?.Close();
            }
        }
    }
}
