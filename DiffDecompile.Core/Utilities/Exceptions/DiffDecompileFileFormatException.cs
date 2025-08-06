using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core.Utilities.Exceptions
{
    [Serializable]
    public class DiffDecompileFileFormatException : Exception
    {
        public DiffDecompileFileFormatException() { }
        public DiffDecompileFileFormatException(string message) : base(message) { }
        public DiffDecompileFileFormatException(string message, Exception inner) : base(message, inner) { }
        protected DiffDecompileFileFormatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
