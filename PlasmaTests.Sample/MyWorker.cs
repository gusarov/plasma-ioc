using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyWorker : IMyWorker
	{
		public int Test
		{
			get { return 777; }
		}
	}
}