using System.Linq;
using System.Collections.Generic;
using System;
using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyService5 : IMyService5
	{
		public bool MyMethod(int a, out int b)
		{
			b = 5;
			return false;
		}
	}
}