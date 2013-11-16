using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyService3))]
	public interface IMyService3
	{
		bool MyMethod();
	}
}