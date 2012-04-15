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
}