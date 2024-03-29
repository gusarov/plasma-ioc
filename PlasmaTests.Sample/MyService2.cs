using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyService2 : IMyService, IMyService2
	{
		readonly IMyService _subService;

		public MyService2(IMyService performer)
		{
			_subService = performer;
		}

		public IMyService SubService
		{
			get { return _subService; }
		}

		public int MyMethod()
		{
			return -1;
		}

		public void MyMethod2()
		{
			
		}
	}
}