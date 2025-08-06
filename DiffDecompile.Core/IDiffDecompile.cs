using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core
{
    public interface IDiffDecompile
    {
        bool IsValid();

        DecompileSourceType SourceType { get; }

        string Magic { get; }

        short Version { get; }

        ushort Count { get; }

        uint DataDirectoryBase { get; }
    }
}
