using System;
using System.Runtime.Serialization;

namespace Plasma
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class PlasmaException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public PlasmaException() {}
		public PlasmaException(string message) : base(message) {}
		public PlasmaException(string message, Exception inner) : base(message, inner) {}

#if !PocketPC
		protected PlasmaException(
			SerializationInfo info,
			StreamingContext context) : base(info, context) {}
#endif
	}

	[Serializable]
	public class StaticCompilerWarning : PlasmaException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public StaticCompilerWarning()
		{
		}

		public StaticCompilerWarning(string message) : base(message)
		{
		}

		public StaticCompilerWarning(string message, Exception inner) : base(message, inner)
		{
		}

		protected StaticCompilerWarning(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}

}