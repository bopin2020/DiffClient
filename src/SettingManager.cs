using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Documents;
using System.Drawing;

#pragma warning disable

namespace DiffClient
{
    internal class SettingManager
    {
        #region Private Members

        private const string _regConfig = "DiffClientProfile";
        private int _size = 0;
        private MainWindowViewModel _mainWindowViewModel;
        private RegistryKey regKey => Registry.CurrentUser;
        private RegistryKey historyKey;
        private RegistryKey rootKey;

        private void QueryAndSetSetting()
        {
            if (_mainWindowViewModel == null) { return; }
            _mainWindowViewModel.Setting = new DiffSetting();
            _mainWindowViewModel.Setting.RemoteUrls = (string[])rootKey.GetValue("RemoteUrls");
            _mainWindowViewModel.Setting.CacheDirectory = (string)rootKey.GetValue("CacheDirectory");
            _mainWindowViewModel.Setting.LogFile = (string)rootKey.GetValue("LogFile");
            _mainWindowViewModel.Setting.IsPreviewDiffDecompile = (string)rootKey.GetValue("IsPreviewDiffDecompile") == "True" ? true : false;
            _mainWindowViewModel.Setting.HistoryNumber = Convert.ToInt16(rootKey.GetValue("HistoryNumber"));
            _mainWindowViewModel.Setting.HistoryDisableFile = (string)rootKey.GetValue("HistoryDisableFile") == "True" ? true : false;
        }

        private void StoreSetting()
        {
            if (rootKey == null) { return; }
            if (_mainWindowViewModel == null) { return; }

            rootKey.SetValue("RemoteUrls", _mainWindowViewModel.Setting.RemoteUrls);
            rootKey.SetValue("CacheDirectory", _mainWindowViewModel.Setting.CacheDirectory);
            rootKey.SetValue("LogFile", _mainWindowViewModel.Setting.LogFile);
            rootKey.SetValue("IsPreviewDiffDecompile", _mainWindowViewModel.Setting.IsPreviewDiffDecompile);
            rootKey.SetValue("HistoryNumber", _mainWindowViewModel.Setting.HistoryNumber);
            rootKey.SetValue("HistoryDisableFile", _mainWindowViewModel.Setting.HistoryDisableFile);
        }

        #endregion

        #region Public Members

        public void InitOrRegisterSetting()
        {
            try
            {
                RegistryKey hreg = null;
                if (!regKey.GetSubKeyNames().Contains(_regConfig))
                {
                    hreg = regKey.CreateSubKey(_regConfig);
                    rootKey = hreg;
                    historyKey = hreg.CreateSubKey("Histories");

                    hreg.SetValue("RemoteUrls", new string[] { });
                    hreg.SetValue("CacheDirectory", "_cache");
                    hreg.SetValue("LogFile", Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Replace("-", "").Substring(8)) + ".log");
                    hreg.SetValue("IsPreviewDiffDecompile", true);
                    hreg.SetValue("HistoryNumber", 10);
                    hreg.SetValue("HistoryDisableFile", true);
                }
                else
                {
                    rootKey = regKey.OpenSubKey(_regConfig, true);
                    historyKey = rootKey.OpenSubKey("Histories", true);
                }
                _size = historyKey.GetValueNames().Length;
                QueryAndSetSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"LoadSetting failed {ex.Message}");
            }

        }
        public void SaveSetting()
        {
            MainWindow.SetStatusException($"MainWindowViewModel.{nameof(SaveSetting)} invoked", LogStatusLevel.Info);
            StoreSetting();
        }
        public int GetHistoryMax()
        {
            return Convert.ToInt32(rootKey.GetValue("HistoryNumber"));
        }

        public void SetHistoryMax(int size)
        {
            rootKey.SetValue("HistoryNumber", size);
        }
        /// <summary>
        /// Least-Recencly-Used LRU Cache Algo
        /// </summary>
        /// <param name="cache"></param>
        public void SetHistory(string cache)
        {
            if (GetHistories().Contains(cache))
            {
                return;
            }

            // step1. check size < capability
            if (_size == 0 || _size < GetHistoryMax())
            {
                historyKey.SetValue(DateTime.Now.ToString(), cache, RegistryValueKind.String);
                _size++;
            }
            else
            {
                // step2. if the size full, we need to use LRU Least Recently Used
                var dt = historyKey.GetValueNames().Select(x => DateTime.Parse(x)).OrderBy(x => x).FirstOrDefault();
                historyKey.DeleteValue(dt.ToString());
                historyKey.SetValue(DateTime.Now.ToString(), cache, RegistryValueKind.String);
            }
        }

        public void SetHistories(string[] caches)
        {
            foreach (string cache in caches)
            {
                historyKey.SetValue(DateTime.Now.ToString(), cache, RegistryValueKind.String);
            }
        }

        public string[] GetHistories()
        {
            List<string> list = new List<string>();
            foreach (string s in historyKey.GetValueNames())
            {
                list.Add((string)historyKey.GetValue(s));
            }
            var result = list.ToArray();
            Array.Reverse(result);
            return result;
        }

        public void Close()
        {
            historyKey.Close();
            rootKey.Close();
        }

        public void ClearHistories()
        {
            foreach (string s in historyKey.GetValueNames())
            {
                historyKey.DeleteValue(s);
            }
        }

        public object GetValue(string key)
        {
            return rootKey.GetValue(key);
        }

        public void SetValue(string key, object value)
        {
            rootKey.SetValue(key,value);
        }

        #endregion

        public SettingManager(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
    }
}
