using DiffClient.Commands;
using DiffClient.DataModel;
using DiffClient.Pages;
using DiffClient.Services;
using DiffClient.UserControls;
using DiffClient.Windows;
using DiffClient.Workflow;
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
using System.Windows.Controls;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient
{
    internal partial class MainWindowViewModel : BaseWindowNotifyModel,IViewModel
    {
        public MainWindowViewModel(MainWindow mainWindow)
        {
            _mainwindows = mainWindow;
            _mainwindows.mainWindowViewModel = this;    
            SettingManager = new SettingManager(this);
            LoadSetting();
            using (var tmp = new AsyncTaskInternal(_mainwindows.progressBar1));
            MainWindowViewModel.PushStrWithGuard("new `MainWindowViewModel`");

            var jobman = new JobManagerPage(mainWindow);
            JobManagerPageModel = jobman.JobmanagerPageModel;
            JobManager = new EnhancedTabItem<GenericView, GenericViewModel>(mainWindow, "Job Manager",jobman);
            JobApi = new Api(_mainwindows);

            for (int i = 0; i < 100; i++)
            {
                JobApi.AddJob($"test_{i}", JobStatus.Pending, "this is a test", "test", JobLevel.Medium);
            }
        }
    }

    internal partial class MainWindowViewModel
    {
        #region Private Members

        private MainWindow _mainwindows;
        private void LoadSetting()
        {
            SettingManager.InitOrRegisterSetting();
        }
        #endregion

        #region Notify Property

        private int _workProgress;
        public int WorkProgress
        {
            get
            {
                return _workProgress;
            }
            set
            {
                _workProgress = value;
                this.OnPropertyChanged("WorkProgress");
            }
        }

        private string statusTip;
        public string StatusToolTip
        {
            get
            {
                return statusTip;
            }

            set
            {
                _mainwindows.g_status.Content = value;
                statusTip = _mainwindows.g_status.Content.ToString();
                this.OnPropertyChanged("StatusToolTip");
            }
        }

        #endregion

        #region Internal Members



        #endregion

        #region Public Members

        public FileStream GlobalLogStream { get; set; }
        public DiffSetting Setting { get; set; }

        public ObservableCollection<DiffTreeItem> TreeItemCaches { get; set; } = new ObservableCollection<DiffTreeItem>();

        public SettingManager SettingManager { get; private set; }

        public static StringBuilder GlobalLogger = new StringBuilder();

        public static object GloablLoggerLock = new object();

        public xxxBuilder xxxBuilderService { get; set; }

        public static Dictionary<string, TabItem> TabControlCached { get; set; } = new Dictionary<string, TabItem>();

        public EnhancedTabItem<GenericView, GenericViewModel> JobManager { get; }
        public JobManagerPageModel JobManagerPageModel { get; }

        public Api JobApi { get; }

        public void SaveSetting()
        {
            SettingManager.SaveSetting();
        }

        /// <summary>
        /// thread-safe  stores global error log info
        /// </summary>
        /// <param name="str"></param>
        public static void PushStrWithGuard(string str)
        {
            Monitor.Enter(GloablLoggerLock);
            {
                GlobalLogger.AppendLine(str);
            }

            Monitor.Exit(GloablLoggerLock);
        }

        #endregion
    }
}
