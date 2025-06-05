using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace DiffClient.Pages
{
    internal class GenericLogPageModel : BasePageModel<GenericLogPage>
    {
        #region Private Members

        private GenericLogPage _genericLogPage;

        #endregion

        #region Notify Propertys

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
                OnPropertyChanged("LogFile");
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
                OnPropertyChanged("EnabledPreviewDiffDecompile");
            }
        }

        private bool showDialog;
        public bool ShowDialog
        {
            get
            {
                return QueryObject.GetSetting(_mainWindow).ShowDialog;
            }
            set
            {
                MainWindow.SetStatusException($"{value} set", LogStatusLevel.Warning);
                _mainWindow.mainWindowViewModel.Setting.ShowDialog = value;
                OnPropertyChanged("ShowDialog");
            }
        }

        #endregion

        public GenericLogPageModel(MainWindow mainWindow, GenericLogPage view) : base(mainWindow, view)
        {
            _genericLogPage = view;
        }
    }
}
