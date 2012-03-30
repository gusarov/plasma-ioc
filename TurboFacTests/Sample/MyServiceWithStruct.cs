using System;

namespace TurboFacTests.Sample
{
	class MyServiceWithStruct
	{
		public MyServiceWithStruct(Guid id)
		{

		}
	}

#if NET4
	class MyServiceWithOptionalStruct
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

	class MyServiceWithOptionalString
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