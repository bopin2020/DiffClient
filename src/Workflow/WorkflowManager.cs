using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace DiffClient.Workflow
{
    internal class WorkflowManager
    {
        #region Private Member

        private MainWindow _mainWindow;

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate ()
            {
                if (_mainWindow.TaskQueues.TryDequeue(out var taskQueues))
                {
                    taskQueues();
                }
            });
        }

        #endregion

        #region Public Members

        public DiffStatus Register()
        {
            _mainWindow.TaskQueues.Enqueue(() =>
            {
                if (Environment.GetCommandLineArgs().Length == 2)
                {
                    _mainWindow.AddDiffDecompileToTreeView(Environment.GetCommandLineArgs()[1],cache: true);
                }
            });
            InitTimer();
            return new DiffStatus();
        }

        public void InitTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Start();
        }

        #endregion

        public WorkflowManager(MainWindow mainwindow)
        {
            _mainWindow = mainwindow;
        }
    }
}
