using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyService : IMyService
	{
		public static int Instantiated;

		public MyService()
		{
			Instantiated++;
		}

		public int MyMethod()
		{
			return Worker.Test;
		}

		[Inject]
		public IMyWorker Worker { get; set; }


	}

	public class MyService6WithoutInterface
	{
		private readonly MyService7Dependency _service;

		public MyService6WithoutInterface(MyService7Dependency service)
		{
			_service = service;
		}

		public MyService7Dependency Service
		{
			get { return _service; }
		}
	}

	public class MyService7Dependency
	{

	}

	public class MyBadServiceDep
	{
		MyBadServiceDep()
		{
			
		}
	}

	public class MyBadService
	{
		public MyBadService(MyBadServiceDep dep)
		{
			
		}
	}
}