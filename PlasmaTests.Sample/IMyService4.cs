using System.Linq;
using System.Collections.Generic;
using System;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyService4))]
	public interface IMyService4
	{
		bool MyMethod();
	}
}