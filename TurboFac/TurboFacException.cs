using System;
using System.Runtime.Serialization;

namespace TurboFac
{
	[Serializable]
	public class TurboFacException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public TurboFacException() {}
		public TurboFacException(string message) : base(message) {}
		public TurboFacException(string message, Exception inner) : base(message, inner) {}

#if !PocketPC
		protected TurboFacException(
			SerializationInfo info,
			StreamingContext context) : base(info, context) {}
#endif
	}
}