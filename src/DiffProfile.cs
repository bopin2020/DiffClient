using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DiffHistoryConfig
    {
        public int MaxNum { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct DiffProfile
    {

    }
}
