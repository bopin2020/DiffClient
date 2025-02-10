using DiffClient.Commands;
using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Automation;

#pragma warning disable

namespace DiffClient
{
    internal partial class MainWindowViewModel : IViewModel, INotifyPropertyChanged
    {
        private MainWindow _mainwindows;
        public MainWindowViewModel(MainWindow mainWindow)
        {
            _mainwindows = mainWindow;
            mainWindow.DataContext = this;
            LoadSetting();
        }
        private string statusTip;
        public string StatusToolTip
        {
            get
            {
                return _mainwindows.g_status.Content.ToString();
            }

            set
            {
                _mainwindows.g_status.Content = value;
                statusTip = _mainwindows.g_status.Content.ToString();
                this.OnPropertyChanged("statusTip");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    internal partial class MainWindowViewModel
    {
        public FileStream GlobalLogStream { get; set; }
        public DiffSetting Setting { get; set; }

        private void LoadSetting()
        {
            if (!File.Exists("resources/setting.profile"))
            {
                Setting = new DiffSetting()
                {
                    RemoteUrls = new string[] { "" },
                    CacheDirectory = "_cache",
                    LogFile = Path.Combine(Path.GetTempPath(),Guid.NewGuid().ToString().Replace("-","").Substring(8)),
                    IsPreviewDiffDecompile = true,
                    HistoryNumber = 10,
                    HistoryDisableFile = true,
                };
                SaveSetting();
            }
            else
            {
                Setting = JsonSerializer.Deserialize<DiffSetting>(File.ReadAllBytes("resources/setting.profile"));
                if(Setting.LogFile != null)
                {
                    GlobalLogStream = new FileStream(Setting.LogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
            }
        }

        public void SaveSetting()
        {
            MainWindow.SetStatusException($"MainWindowViewModel.{nameof(SaveSetting)} invoked", LogStatusLevel.Info);
            File.WriteAllText("resources/setting.profile", JsonSerializer.Serialize(Setting, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
