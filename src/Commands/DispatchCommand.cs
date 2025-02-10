using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

#pragma warning disable 8602
#pragma warning disable 8605

namespace DiffClient.Commands
{
    internal class DispatchCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public DispatchCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            DispatchEvent even = (DispatchEvent)parameter;
            MainWindow.SetStatusException($"{nameof(DispatchCommand)} {even.ToString()} invoked", LogStatusLevel.Info);
            switch (even)
            {
                case DispatchEvent.CleanTablItems:
                    int? max = _mainWindow.rootTab?.Items.Count;
                    for (int i = 1; i < max; i++)
                    {
                        _mainWindow.rootTab?.Items.RemoveAt(i);
                    }
                    _mainWindow.rootTab.SelectedIndex = 0;
                    break;
                case DispatchEvent.ExitApp:
                    break;
                case DispatchEvent.OpenSetting:
                    _mainWindow.rootTab?.TabControlAddAndSelect(new EnhancedTabItem<PerformanceView, PerformanceViewModel>(_mainWindow, "Setting", _mainWindow));
                    break;
                case DispatchEvent.AccessCloud:
                    int index = 0;
                    foreach (var item in _mainWindow?.mainWindowViewModel?.Setting?.RemoteUrls)
                    {
                        MainWindow.SetStatusException($"AccessCloud {item}", LogStatusLevel.Warning);
                        foreach (var entry in CloudDiffDecomile.ParseFromUrl(item + "/index.json").Items)
                        {
                            QueryObject.GetIndexTreeView(_mainWindow).Items.Add(new DiffTreeItem() { Header = entry, Foreground = Brushes.DarkGray, FullPath = _mainWindow.mainWindowViewModel.Setting.RemoteUrls[index] + $"/{entry}", Cloud = new CloudModel { Initialized = false, IsCloud = true } });
                        }
                        index++;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
