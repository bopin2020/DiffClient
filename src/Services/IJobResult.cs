using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Services
{
    internal interface IJobResult
    {
        long Status { get; }
    }
}
