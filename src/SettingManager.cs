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

#pragma warning disable 0252
#pragma warning disable 8600
#pragma warning disable 8601
#pragma warning disable 8604
#pragma warning disable 8605

namespace DiffClient
{
    internal class SettingManager
    {
        private const string _regConfig = "DiffClientProfile";
        private MainWindowViewModel _mainWindowViewModel;
        private RegistryKey regKey => Registry.CurrentUser;
        private RegistryKey historyKey;
        private RegistryKey rootKey;

        public SettingManager(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        private void QueryAndSetSetting()
        {
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
            rootKey.SetValue("RemoteUrls", _mainWindowViewModel.Setting.RemoteUrls);
            rootKey.SetValue("CacheDirectory", _mainWindowViewModel.Setting.CacheDirectory);
            rootKey.SetValue("LogFile", _mainWindowViewModel.Setting.LogFile);
            rootKey.SetValue("IsPreviewDiffDecompile", _mainWindowViewModel.Setting.IsPreviewDiffDecompile);
            rootKey.SetValue("HistoryNumber", _mainWindowViewModel.Setting.HistoryNumber);
            rootKey.SetValue("HistoryDisableFile", _mainWindowViewModel.Setting.HistoryDisableFile);
        }

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
                    rootKey = regKey.OpenSubKey(_regConfig);
                    historyKey = rootKey.OpenSubKey("Histories");
                }
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
            if(rootKey.GetValueKind("HistoryNumber") == RegistryValueKind.DWord)
            {
                return (int)rootKey.GetValue("HistoryNumber");
            }
            return 0;
        }

        public void SetHistoryMax(int size)
        {
            rootKey.SetValue("HistoryNumber", size);
        }

        public void SetHistories(string diffdecompile)
        {
            int count = historyKey.GetValueNames().Length;
        }

        public string[] GetHistories()
        {
            List<string> list = new List<string>();
            foreach (string s in historyKey.GetValueNames())
            {
                list.Add((string)historyKey.GetValue(s));
            }
            return list.ToArray();
        }

        public void Close()
        {
            historyKey.Close();
            rootKey.Close();
        }
    }
}
