using DiffClient.UserControls;
using DiffClient.Windows;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

#pragma warning disable

namespace DiffClient.Commands
{
    public class ExitCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public bool Cancel { get; set; }

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
            if(!ExitFromClick(false))
                Environment.Exit(0);
        }

        public bool ExitFromClick(bool click)
        {
            MainWindow.SetStatusException($"{nameof(ExitCommand)} invoked", LogStatusLevel.Warning);
            int flag = (int)_mainWindow.mainWindowViewModel.SettingManager.GetValue("ShowDialog");
            if (flag == 0)
            {
                return false;
            }

            var window = new ExitDialogWindow(_mainWindow,this);
            var viewmodel = new ExitDialogWindowViewModel(window);
            window.DataContext = viewmodel;
            window.ShowDialog();
            return Cancel;
        }
    }
}
