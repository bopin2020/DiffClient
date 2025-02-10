using DiffClient.UserControls;
using DiffClient.Utility;
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

#pragma warning disable 1591
#pragma warning disable 8600

namespace DiffClient.Commands
{
    internal class FileCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public FileCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            MainWindow.SetStatusException($"{nameof(FileCommand)} invoked", LogStatusLevel.Warning);
            TabItem indexpageview = _mainWindow?.rootTab.Items.Cast<TabItem>().Where(x => x.Content.GetType().Name == "IndexPageView").FirstOrDefault();
            if(indexpageview != null)
            {
                IndexPageView view = indexpageview.Content as IndexPageView;
                foreach (var item in DiffClientUtility.OpenDiffDecompileFiles())
                {
                    _mainWindow.AddDiffDecompileToTreeView(item, true);
                }
            }
        }
    }
}
