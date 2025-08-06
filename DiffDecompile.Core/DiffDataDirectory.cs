using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DiffDataDirectory
    {
        public int VirtualAddress { get; internal set; }

        public int Size { get; internal set; }
    }
}
