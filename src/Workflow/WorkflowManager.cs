using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Workflow
{
    internal class WorkflowManager
    {
        public MainWindow mainWindow;

        public WorkflowManager(MainWindow _mainwindow)
        {
            mainWindow = _mainwindow;
        }

        public DiffStatus Register()
        {
            mainWindow.TaskQueues.Enqueue(() =>
            {
                if (Environment.GetCommandLineArgs().Length == 2)
                {
                    mainWindow.AddDiffDecompileToTreeView(Environment.GetCommandLineArgs()[1]);
                }
            });

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick; ;
            dispatcherTimer.Interval = new TimeSpan(0,0, 0, 0,50);
            dispatcherTimer.Start();
            return new DiffStatus();
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate ()
            {
                if (mainWindow.TaskQueues.TryDequeue(out var taskQueues))
                {
                    taskQueues();
                }
            });
        }
    }
}
