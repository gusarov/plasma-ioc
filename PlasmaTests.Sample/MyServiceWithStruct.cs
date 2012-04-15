using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyServiceWithStruct
	{
		public MyServiceWithStruct(Guid id)
		{

		}
	}

#if NET4
	[RegisterService]
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

	[RegisterService]
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