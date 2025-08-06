using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core
{
    public class DiffDecompileFileFormat
    {
        /// <summary>
        /// SourceType
        /// </summary>
        public DecompileSourceType SourceType { get; internal set; }
        /// <summary>
        /// reserved it must be zero
        /// </summary>
        public byte Reserved { get; internal set; }
        /// <summary>
        /// diffdecompile\0
        /// </summary>
        public string Magic { get; internal set; }
        /// <summary>
        /// 2 bytes version
        /// </summary>
        public short Version { get; internal set; }
        /// <summary>
        /// matched function diff count 
        /// </summary>
        public ushort Count { get; internal set; }

        /// <summary>
        /// decompile code offset in data directory
        /// </summary>
        public uint DataDirectoryBase { get; internal set; }
        /// <summary>
        /// List DiffDecompileEntryFormat
        /// </summary>
        public List<DiffDecompileEntryFormat> Entries { get; internal set; } = new List<DiffDecompileEntryFormat>();
    }
}
