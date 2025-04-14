using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Services
{
    internal class xxxBuilder
    {
        private MainWindow mainWindow;

        public IProcessReporter ProcessReporter { get; set; }
        public IJobContext JobContext { get; set; }

        public xxxBuilder(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            ProcessReporter = TabProgressReporter.Instance;
            JobContext = new JobContextObj(this.mainWindow);
        }

        public async Task<IJobResult> Buildxxx()
        {
            var result = await ProcessReporter.ReportProgress(JobContext);


            return result;
        }
    }
}
