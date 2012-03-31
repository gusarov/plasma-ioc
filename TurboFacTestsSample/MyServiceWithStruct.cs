using System;

using TurboFac;

namespace TurboFacTests.Sample
{
	[TurboReg]
	public class MyServiceWithStruct
	{
		public MyServiceWithStruct(Guid id)
		{

		}
	}

#if NET4
	[TurboReg]
	public class MyServiceWithOptionalStruct
	{
		readonly bool _val;

		public MyServiceWithOptionalStruct(bool val = true)
		{
			_val = val;
		}

		public bool Val
		{
			get { return _val; }
		}
	}

	[TurboReg]
	public class MyServiceWithOptionalString
	{
		readonly string _val;

		public MyServiceWithOptionalString(string val = "test")
		{
			_val = val;
		}

		public string Val
		{
			get { return _val; }
		}
	}
#endif

}