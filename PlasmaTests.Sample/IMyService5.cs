using System.Linq;
using System.Collections.Generic;
using System;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyService5))]
	public interface IMyService5
	{
		bool MyMethod(int a, out int b);
	}
}