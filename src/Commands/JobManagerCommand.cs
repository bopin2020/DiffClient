using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffClient.Windows;
using System.Windows.Input;
using DiffClient.Pages;

#pragma warning disable

namespace DiffClient.Commands
{
    internal enum JobManagerRouteEvent
    {
        [Description("Add Job")]
        AddJob,
        [Description("Cancel Job")]
        CancelJob,
        [Description("Suspend Job")]
        SuspendJob,
        [Description("Resume Job")]
        ResumeJob,
        [Description("Clean Job")]
        CleanJob,
        [Description("Restart Job")]
        RestartJob,
        [Description("Grouping")]
        Grouping,
        [Description("Update JobLevel")]
        UpdateJobLevel
    }

    internal class JobManagerCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private MainWindow _mainWindow;

        public JobManagerCommand(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var ctx = parameter as ContextMenuContext;
            if (ctx.DataGrid == null)
            {
                return;
            }

            JobManagerRouteEvent re = (JobManagerRouteEvent)ctx.CommandParameter;
            var jpm = ctx.ViewAndViewModelContext as JobManagerPageModel;
            if (jpm == null)
            {
                return;
            }
            var currow = ctx.DataGrid.SelectedItem as ItemModel;
            if (currow == null) 
            {
                return;
            }

            switch (re)
            {
                case JobManagerRouteEvent.AddJob:
                    break;
                case JobManagerRouteEvent.CancelJob:
                    jpm.SetValue(currow, new KeyValuePair<string, object>("JobStatus", JobStatus.Cancel));
                    break;
                case JobManagerRouteEvent.SuspendJob:
                    jpm.SetValue(currow, new KeyValuePair<string, object>("JobStatus", JobStatus.Pending));
                    break;
                case JobManagerRouteEvent.ResumeJob:
                    jpm.SetValue(currow, new KeyValuePair<string, object>("JobStatus", JobStatus.Running));
                    break;
                case JobManagerRouteEvent.CleanJob:
                    jpm.SetValue(currow, new KeyValuePair<string, object>("JobStatus", JobStatus.Abort));
                    break;
                case JobManagerRouteEvent.RestartJob:
                    jpm.SetValue(currow, new KeyValuePair<string, object>("JobStatus", JobStatus.Init));
                    break;
                case JobManagerRouteEvent.Grouping:
                    break;
                case JobManagerRouteEvent.UpdateJobLevel:
                    jpm.SetValue(currow, new KeyValuePair<string, object>("JobLevel", JobLevel.System));
                    break;
                default:
                    break;
            }

            ctx.DataGrid.Items.Refresh();
        }
    }

}
