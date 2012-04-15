using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyServiceWithSeveralCtors
	{
		readonly IMyService _service1;
		readonly IMyService _service2;
		readonly IMyService _service3;

		public MyServiceWithSeveralCtors(IMyService service1, IMyService service2, IMyService service3)
		{
			_service1 = service1;
			_service2 = service2;
			_service3 = service3;
		}

		[DefaultConstructor]
		public MyServiceWithSeveralCtors(IMyService service1, IMyService service2)
		{
			_service1 = service1;
			_service2 = service2;
		}

		public MyServiceWithSeveralCtors(IMyService service1)
		{
			_service1 = service1;
		}

		public IMyService Service1
		{
			get { return _service1; }
		}

		public IMyService Service2
		{
			get { return _service2; }
		}

		public IMyService Service3
		{
			get { return _service3; }
		}
	}
}
