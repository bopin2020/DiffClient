using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Services
{
    internal class JobResult : IJobResult
    {
        private long id;

        private const long _success = 0x0;
        private const long _failed = 0xfffffffe;

        public static JobResult SuccInstance { get; } = new JobResult(_success);
        public static JobResult FailedInstance { get; } = new JobResult(_failed);

        public JobResult(long id)
        {
            this.id = id;
        }

        public long Status => id;
    }
}
