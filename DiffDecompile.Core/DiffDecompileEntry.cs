using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core
{
    [Description("text-based diff decompile function code")]
    public class DecompileUnit
    {
        public DecompileSourceType SourceType { get; internal set; }
    }

    public class DiffDecompileEntry : DecompileUnit
    {
        public ushort Flag { get; internal set; }
        public int Id { get; internal set; }
        public float Similarity { get; internal set; }

        public float Confidence { get; internal set; }

        public string PrimaryName { get; internal set; }

        public long PrimaryAddress { get; internal set; }

        public byte[] PrimaryData { get; internal set; }

        public Lazy<string> PrimaryDecompileCode => new Lazy<string>(() => Encoding.UTF8.GetString(PrimaryData));

        public string SecondaryName { get; internal set; }

        public long SecondaryAddress { get; internal set; }

        public byte[] SecondaryData { get; internal set; }
        public Lazy<string> SecondaryDecompileCode => new Lazy<string>(() => Encoding.UTF8.GetString(SecondaryData));
    }
}
