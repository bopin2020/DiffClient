using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core
{
    public class DiffDecompileEntryFormat
    {
        public ushort Flag { get;internal set; }
        public ushort Id { get; internal set; }
        public float Similarity { get; internal set; }

        public float Confidence { get; internal set; }

        public ushort PrimaryLen { get; internal set; }

        public string PrimaryName { get; internal set; }

        public long PrimaryAddress { get; internal set; }

        public DiffDataDirectory PrimaryDataDirectory { get; internal set; }

        public ushort SecondaryLen { get; internal set; }

        public string SecondaryName { get; internal set; }

        public long SecondaryAddress { get; internal set; }
        public DiffDataDirectory SecondaryDataDirectory { get; internal set; }

        public static implicit operator DiffDecompileEntry(DiffDecompileEntryFormat df)
        {
            return new DiffDecompileEntry()
            {
                Flag = df.Flag,
                Id = df.Id,
                Similarity = df.Similarity,
                Confidence = df.Confidence,
                PrimaryName = df.PrimaryName,
                PrimaryAddress = df.PrimaryAddress,
                SecondaryName = df.SecondaryName,
                SecondaryAddress = df.SecondaryAddress,
            };
        }
    }
}
