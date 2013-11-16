using System.Linq;
using System.Collections.Generic;
using System;

namespace PlasmaTests.Sample
{
	[DefaultImpl(typeof(MyService5))]
	public interface IMyServiceComplex
	{
		bool MyMethod(int a, out int b);
		bool MyMethod(int[] a, object[] b, IComparable[] c);
		//IDictionary<string, string> test{get;}
	}
}