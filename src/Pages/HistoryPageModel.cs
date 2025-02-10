using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Pages
{
    internal class HistoryPageModel : BasePageModel<HistoryPage>
    {
        public HistoryPageModel(MainWindow mainWindow, HistoryPage Page) : base(mainWindow, Page) { }

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
                OnPropertyChanged("_historyNumber");
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
                OnPropertyChanged("historyDisableFile");
            }
        }
    }
}
