using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyService))]
	public interface IMyService
	{
		int MyMethod();
	}
}