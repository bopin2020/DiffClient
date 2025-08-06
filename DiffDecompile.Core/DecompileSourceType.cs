using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core
{
    public enum DecompileSourceType : byte
    {
        None = 0x00,
        IDAPro = 0x1,
        BinaryNinja = 0x02,
        Ghidra = 0x03,
        Angr = 0x04,
        Unknown = 0x7f,
        /// <summary>
        /// error  above 0x80
        /// </summary>
        Error = 0x80,
    }
}
