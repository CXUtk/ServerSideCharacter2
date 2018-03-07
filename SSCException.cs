using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2
{
	[Serializable]
	public class SSCException : Exception
	{
		public SSCException() : base() { }
		public SSCException(string message) : base(message) { }
		public SSCException(string message, Exception inner) : base(message, inner) { }
		protected SSCException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
