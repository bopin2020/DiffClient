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

namespace DiffClient.Commands
{
    internal class HighlightTreeViewItemCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        private List<Brush> _cacheColor = typeof(Brushes).GetProperties().Select(x => (Brush)x.GetValue(null)).ToList();

        private List<DiffTreeItem> _cacheHasHighlightitems = new List<DiffTreeItem>();

        public HighlightTreeViewItemCommand(MainWindow mainWindow)
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
            if (_cacheHasHighlightitems.Contains(tmp))
            {
                tmp.Background = tmp.DefaultBackground;
                return;
            }
            _cacheHasHighlightitems.Add(tmp);
            tmp.LastBackground = tmp.Background;
            Random random = new Random();
            int index = random.Next(0, _cacheColor.Count);
            tmp.Background = _cacheColor[index];
            _cacheColor.RemoveAt(index);
        }
    }
}
