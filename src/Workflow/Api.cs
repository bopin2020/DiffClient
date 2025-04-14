using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffClient.Pages;

namespace DiffClient.Workflow
{
    internal class Api
    {
        private int _jid = 0;
        private MainWindow _mainWindow;
        private object _locker = new object();

        public Api(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void AddDefaultJob(string jobName,string description) => AddDefaultJob(jobName,description,"");
        public void AddDefaultJob(string jobname,string description,string group) => AddJob(jobname, JobStatus.Pending,description, group,JobLevel.Medium);

        public void AddJob(string jobname,JobStatus jobStatus,string description,string group,JobLevel jobLevel)
        {
            Monitor.Enter(_locker);
            _mainWindow.mainWindowViewModel.JobManagerPageModel.JobItems.Add(new ItemModel
            {
                Attributes =
                            {
                                ["Name"] = jobname,
                                ["Jid"] = _jid,
                                ["JobStatus"] = jobStatus,
                                ["Description"] = description,
                                ["StartTime"] = DateTime.Now.ToString(),
                                ["EndTime"] = "",
                                ["Group"] = group,
                                ["JobLevel"] = jobLevel,
                            }
            });
            _jid++;
            Monitor.Exit(_locker);
        }
    }
}
