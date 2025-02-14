using DiffClient.Commands;
using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
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
            SettingManager = new SettingManager(this);
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

        public ObservableCollection<DiffTreeItem> TreeItemCaches { get; set; } = new ObservableCollection<DiffTreeItem>();

        public SettingManager SettingManager { get; private set; }


        private void LoadSetting()
        {
            SettingManager.InitOrRegisterSetting();
        }

        public void SaveSetting()
        {
            SettingManager.SaveSetting();
        }
    }
}
