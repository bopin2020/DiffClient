using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Database
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DiffDBFormat
    {
        /*
         TabControl  items
                    current item, position

        TreeView

        Frame   items
         */
        public uint Magic;

        public byte[] Data;

        public string FileName;
    }
}
