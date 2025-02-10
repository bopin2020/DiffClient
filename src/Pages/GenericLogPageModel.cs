using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Pages
{
    internal class GenericLogPageModel : BasePageModel<GenericLogPage>
    {
        public GenericLogPageModel(MainWindow mainWindow, GenericLogPage view) : base(mainWindow, view)
        {
        }

        private string _logfile;
        public string LogFile
        {
            get
            {
                return QueryObject.GetSetting(_mainWindow).LogFile;
            }
            set
            {
                MainWindow.SetStatusException($"{value} set", LogStatusLevel.Warning);
                _mainWindow.mainWindowViewModel.Setting.LogFile = value;
                OnPropertyChanged("_logfile");
            }
        }

        private bool enabledPreviewDiffDecompile;
        public bool EnabledPreviewDiffDecompile
        {
            get
            {
                return QueryObject.GetSetting(_mainWindow).IsPreviewDiffDecompile;
            }
            set
            {
                MainWindow.SetStatusException($"{value} set", LogStatusLevel.Warning);
                _mainWindow.mainWindowViewModel.Setting.IsPreviewDiffDecompile = value;
                OnPropertyChanged("enabledPreviewDiffDecompile");
            }
        }
    }
}
