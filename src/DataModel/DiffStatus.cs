using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.DataModel
{
    internal sealed class DiffStatus
    {
        public ulong ErrorCode { get; internal set; }
        public string Description { get; internal set; }
    }
}
