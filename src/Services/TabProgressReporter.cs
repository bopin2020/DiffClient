using DiffClient.UserControls;
using RichTextBoxLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

#pragma warning disable

namespace DiffClient.Services
{
    internal class TabProgressReporter : IProcessReporter
    {
        private int _count = 0;
        public static TabProgressReporter Instance { get; } = new TabProgressReporter();
        public async Task<IJobResult> ReportProgress(IJobContext context)
        {
            if (!MainWindowViewModel.TabControlCached.ContainsKey("Log"))
            {
                return await Task.FromResult<IJobResult>(JobResult.FailedInstance);
            }
            if(MainWindowViewModel.TabControlCached.TryGetValue("Log",out var tabItem))
            {
                GlobalLoggerView glv = tabItem.Content as GlobalLoggerView;
                if (glv!=null)
                {
                    ParagraphRun pc = new ParagraphRun();
                    glv.richtextbox.Document.Blocks.Add(pc.AddStr($"{nameof(TabProgressReporter)} {_count}", Brushes.Green));

                    ParagraphRun pc2 = new ParagraphRun();
                    glv.richtextbox.Document.Blocks.Add(pc.AddStr($"{context.Message} {_count}", Brushes.Black));
                }
            }
            _count++;
            return await Task.FromResult<IJobResult>(JobResult.SuccInstance);
        }
    }
}
