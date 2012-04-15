using System.Linq;
using System.Collections.Generic;
using System;

namespace PlasmaTests.Sample
{
	public interface IMyService2
	{
		void MyMethod2();
		IMyService SubService { get; }
	}
}