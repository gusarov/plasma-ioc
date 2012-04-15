using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyWorker))]
	public interface IMyWorker
	{
		int Test { get; }
	}
}