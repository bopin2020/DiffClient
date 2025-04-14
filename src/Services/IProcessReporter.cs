using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Services
{
    

    internal interface IProcessReporter
    {
        Task<IJobResult> ReportProgress(IJobContext context);
    }
}
