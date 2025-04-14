using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

#pragma warning disable

namespace DiffClient.Workflow
{
    internal class AsyncTaskInternal : IDisposable
    {
        #region Private Members

        private BackgroundWorker backgroundWorker;
        private ProgressBar progressBar;

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            for (int i = 0; i <= 100; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                Thread.Sleep(10);

                worker.ReportProgress(i);
            }

        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                progressBar.Value = 0;
            }
            else if (e.Error != null)
            {
                progressBar.Value = 0;
            }
            else
            {

            }
        }

        #endregion

        #region Public Members

        public void Dispose()
        {
            progressBar.Value = -1;
            backgroundWorker.ReportProgress(-1);
        }

        #endregion

        public AsyncTaskInternal(ProgressBar _bar)
        {
            this.progressBar = _bar;
            InitializeBackgroundWorker();
        }
    }
}
