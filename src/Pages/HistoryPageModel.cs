using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace DiffClient.Pages
{
    internal class HistoryItem
    {
        public int Index { get; set; }
        public string Value { get; set; }
    }
    internal class HistoryPageModel : BasePageModel<HistoryPage>
    {
        #region Private Members

        private int _count = 0;
        private HistoryPage _historyPage;

        private void initHistories()
        {
            foreach (var item in _mainWindow.mainWindowViewModel.SettingManager.GetHistories())
            {
                HistoriesData.Add(new HistoryItem
                {
                    Index = _count,
                    Value = item
                });
                _count++;
            }
        }
        #endregion

        #region Notify Propertys

        private short _historyNumber;
        public short HistoryNumber
        {
            get
            {
                return _mainWindow.mainWindowViewModel.Setting.HistoryNumber;
            }
            set
            {
                MainWindow.SetStatusException($"{value} set", LogStatusLevel.Warning);
                _mainWindow.mainWindowViewModel.Setting.HistoryNumber = value;
                OnPropertyChanged("HistoryNumber");
            }
        }
        private bool historyDisableFile;
        public bool HistoryDisableFile
        {
            get
            {
                return _mainWindow.mainWindowViewModel.Setting.HistoryDisableFile;
            }
            set
            {
                _mainWindow.mainWindowViewModel.Setting.HistoryDisableFile = value;
                OnPropertyChanged("HistoryDisableFile");
            }
        }

        #endregion

        #region Public Members

        public ObservableCollection<HistoryItem> HistoriesData { get; set; } = new ObservableCollection<HistoryItem>();

        #endregion

        public HistoryPageModel(MainWindow mainWindow, HistoryPage Page) : base(mainWindow, Page)
        {
            _historyPage = Page;
            Page.historyview.ItemsSource = HistoriesData;
            initHistories();
        }
    }
}
