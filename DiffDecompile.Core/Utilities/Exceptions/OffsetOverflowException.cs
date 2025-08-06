using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffDecompile.Core.Utilities.Exceptions
{
	[Serializable]
	public class OffsetOverflowException : Exception
	{
		public OffsetOverflowException() { }
		public OffsetOverflowException(string message) : base(message) { }
		public OffsetOverflowException(string message, Exception inner) : base(message, inner) { }
		protected OffsetOverflowException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
