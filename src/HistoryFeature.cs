using DiffClient.Commands;
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

namespace DiffClient
{
    internal class HistoryCacheEntry
    {
        public string FileName { get; }

        public HistoryCacheEntry(string filename)
        {
            FileName = filename;
        }
    }

    /// <summary>
    /// non-thread safe when initialize cache and insert operation
    /// </summary>
    internal class HistoryFeature : IDisposable
    {
        private string historyfile = $@"./resources/history.txt";
        private MainWindow mainWindow;
        private ICommand command;
        private int _count;
        private List<HistoryCacheEntry> _cachehistory = new List<HistoryCacheEntry>();
        public HistoryFeature(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            command = new HistoryCommand(mainWindow);
        }
        /// <summary>
        /// todo  bug fix??
        /// </summary>
        public void initHistoryMenuItem()
        {
            mainWindow.TaskQueues.Enqueue(() =>
            {
                foreach (var item in mainWindow.mainWindowViewModel.SettingManager.GetHistories())
                {
                    try
                    {
                        if (QueryObject.GetSetting(mainWindow).HistoryDisableFile)
                        {
                            if (!File.Exists(item))
                            {
                                continue;
                            }
                        }
                        int size = QueryObject.GetSetting(mainWindow).HistoryNumber;
                        if (_count > size && size != 0)
                        {
                            break;
                        }
                        HistoryCacheEntry hce = new(item);
                        _cachehistory.Add(hce);
                        mainWindow.OpenHistoryMenuItem.Items.Add(new MenuItem()
                        {
                            Header = hce.FileName,
                            Command = command,
                            CommandParameter = hce.FileName,
                            Foreground = !File.Exists(hce.FileName) ? Brushes.Red : Brushes.Black
                        });
                        _count++;
                        MainWindow.SetStatusException($"HistoryFeature init cache history MenuItem {hce.FileName}", LogStatusLevel.Warning);
                    }
                    catch (Exception ex)
                    {
                        DiffClientUtility.ExceptionHandled(ex, "init history", item);
                    }
                }
            });
        }

        public void AddCache(HistoryCacheEntry cacheEntry)
        {
            _cachehistory.Add(cacheEntry);
            MainWindow.SetStatusException($"HistoryFeature AddCache {cacheEntry.FileName}", LogStatusLevel.Warning);
        }

        public void Clear()
        {
            _cachehistory.Clear();
            MainWindow.SetStatusException($"HistoryFeature cache clear", LogStatusLevel.Warning);
        }

        public void Update()
        {
            if (_cachehistory.Count > 0)
            {
                File.WriteAllLines(historyfile, _cachehistory.Where(x => x.FileName != "").Select(x => x.FileName).ToArray());
                MainWindow.SetStatusException($"HistoryFeature write finished", LogStatusLevel.Warning);
            }
        }

        public void Dispose()
        {
            Update();
        }
    }
}
