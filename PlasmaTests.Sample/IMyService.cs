using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyService))]
	[DefaultImpl("MyService asdas ")]
	[DefaultImpl("MyService asd as")]
	public interface IMyService
	{
		int MyMethod();
	}
}