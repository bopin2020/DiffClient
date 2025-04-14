using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Services
{
    internal class NullProgressReporter : IProcessReporter
    {
        public static NullProgressReporter Instance { get; } = new NullProgressReporter();
        public async Task<IJobResult> ReportProgress(IJobContext context)
        {
            return await Task.FromResult<IJobResult>(JobResult.SuccInstance);
        }
    }
}
