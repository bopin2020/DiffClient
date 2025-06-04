using DiffClient.DataModel;
using DiffClient.Pages;
using DiffClient.UserControls;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

#pragma warning disable

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
                    _mainWindow.rootTab?.TabControlAddAndSelect(
                                                                new EnhancedTabItem<PerformanceView, PerformanceViewModel>
                                                                (_mainWindow, "Setting", _mainWindow),
                                                                "Setting"
                                                                );
                    break;
                case DispatchEvent.AccessCloud:
                    int index = 0;
                    _mainWindow.mainWindowViewModel.JobApi.AddJob("accesscloud",JobStatus.Pending, "download diff decompile file", "download", JobLevel.High);
                    foreach (var item in _mainWindow?.mainWindowViewModel?.Setting?.RemoteUrls)
                    {
                        MainWindow.SetStatusException($"AccessCloud {item}", LogStatusLevel.Warning);
                        var entry = CloudDiffDecomile.ParseFromUrl(item + "/index.json");
                        MainWindow.SetStatusException($"{entry.Date} Count: {entry.Count}", LogStatusLevel.Info);
                        foreach (var ele_os in entry.Items)
                        {
                            foreach (var ele_mon in ele_os.Items)
                            {
                                foreach(var ele in ele_mon.Items)
                                {
                                    var treeitem = new DiffTreeItem()
                                    {
                                        Header = ele.File,
                                        OS = ele_os.OS,
                                        Date = ele_mon.Date,
                                        IsLocal = false,
                                        Foreground = Brushes.DarkGray,
                                        FullPath = _mainWindow.mainWindowViewModel.Setting.RemoteUrls[index] + $"/data/{ele_os.OS}/{ele_mon.Date}/{ele.File}",
                                        Cloud = new CloudModel
                                        {
                                            Initialized = false,
                                            IsCloud = true
                                        }
                                    };
                                    Func<string> call = (() =>
                                    {
                                        return new TreeToolTipBuilder(new FileInfo(ele.File).Name).ToString();
                                    });

                                    treeitem.ToolTip = call.EnterGuard();
                                    _mainWindow.mainWindowViewModel.TreeItemCaches.Add(treeitem);
                                    QueryObject.GetIndexPageViewContent(_mainWindow).ViewModel.TreeItemsSource.Add(treeitem);
                                }
                            }
                        }
                        index++;
                    }
                    break;
                case DispatchEvent.OpenLog:
                    _mainWindow.rootTab?.TabControlAddAndSelect(
                                                                new EnhancedTabItem<GlobalLoggerView, GlobalLoggerViewModel>
                                                                (_mainWindow, "Log", _mainWindow),
                                                                "Log"
                                                                );
                    break;
                case DispatchEvent.Dynamic:
                    _mainWindow.rootTab?.TabControlAddAndSelect(
                                                                new EnhancedTabItem<DynamicColumnsView, DynamicColumnsViewModel>
                                                                (_mainWindow, "Dynamic", _mainWindow),
                                                                "Dynamic"
                                                                );
                    break;
                case DispatchEvent.JobManager:
                    _mainWindow.rootTab?.TabControlAddAndSelect(_mainWindow.mainWindowViewModel.JobManager,"Job Manager");
                    break;
                case DispatchEvent.AccessLocalStore:
                    string dir = "D:\\Desktop\\diffdecompile\\data";
                    _mainWindow.AddDirectoryToTreeView(dir);
                    break;
                case DispatchEvent.Restart:
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Process.GetCurrentProcess().MainModule.FileName,
                        UseShellExecute = true
                    });
                    App.Current.Shutdown();
                    break;
                case DispatchEvent.ClearHistories:
                    {
                        _mainWindow.HistoryFeatureInstance.Clear(hard: true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
