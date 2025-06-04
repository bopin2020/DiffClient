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

        public bool IsNew { get; set; }

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
        #region Private Members

        private MainWindow mainWindow;
        private ICommand command;
        private int _count;
        private List<HistoryCacheEntry> _cachehistory = new List<HistoryCacheEntry>();

        #endregion

        #region Public Members

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
                        int size = QueryObject.GetSetting(mainWindow).HistoryNumber;
                        if (_count >= size && size != 0)
                        {
                            break;
                        }
                        HistoryCacheEntry hce = new(item);
                        _cachehistory.Add(hce);
                        hce.IsNew = false;
                        mainWindow.OpenHistoryMenuItem.Items.Add(new MenuItem()
                        {
                            Header = hce.FileName,
                            Command = command,
                            CommandParameter = hce.FileName,
                            Foreground = File.Exists(hce.FileName) ? Brushes.Black : Brushes.Red,
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

        public void AddCache(HistoryCacheEntry cacheEntry,bool hard = false)
        {
            if (hard)
            {
                mainWindow.mainWindowViewModel.SettingManager.SetHistory(cacheEntry.FileName);
            }
            else
            {
                _cachehistory.Add(cacheEntry);
            }
            MainWindow.SetStatusException($"HistoryFeature AddCache {cacheEntry.FileName}", LogStatusLevel.Warning);
        }

        public void Clear(bool hard = false)
        {
            _cachehistory.Clear();
            if (hard)
            {
                mainWindow.mainWindowViewModel.SettingManager.ClearHistories();
            }
            MainWindow.SetStatusException($"HistoryFeature cache clear", LogStatusLevel.Warning);
        }

        public void Update()
        {
            if (_cachehistory.Count > 0)
            {
                // do not write cache repeatly
                mainWindow.mainWindowViewModel.SettingManager.SetHistories(_cachehistory.Where(x => x.FileName != "").Select(x => x.FileName).ToArray());
                MainWindow.SetStatusException($"HistoryFeature write finished", LogStatusLevel.Warning);
            }
        }

        public void Dispose()
        {
            Update();
        }

        #endregion

        public HistoryFeature(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            command = new HistoryCommand(mainWindow);
        }
    }
}
