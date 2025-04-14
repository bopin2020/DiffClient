using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace DiffClient.Pages
{
    internal class SettingCloudPageModel : BasePageModel<SettingCloudPage>
    {
        #region Private Members


        #endregion

        #region Notify Propertys

        private string remoteUrls;
        public string RemoteUrls
        {
            get
            {
                return string.Join(";", _mainWindow.mainWindowViewModel.Setting.RemoteUrls);
            }
            set
            {
                MainWindow.SetStatusException($"{value} set", LogStatusLevel.Warning);
                _mainWindow.mainWindowViewModel.Setting.RemoteUrls = value.Split(';');
                OnPropertyChanged("remoteUrls");
            }
        }

        private string cachedir;
        public string CacheDir
        {
            get
            {
                return _mainWindow.mainWindowViewModel.Setting.CacheDirectory;
            }
            set
            {
                _mainWindow.mainWindowViewModel.Setting.CacheDirectory = value;
                OnPropertyChanged("cachedir");
            }
        }

        #endregion

        #region Public Members



        #endregion

        public SettingCloudPageModel(MainWindow mainWindow,SettingCloudPage cloudPage) : base(mainWindow, cloudPage) { }

    }
}
