using System.Linq;
using System.Collections.Generic;
using System;
using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyService5 : IMyServiceComplex
	{
		public bool MyMethod(int a, out int b)
		{
			b = 5;
			return false;
		}


		public bool MyMethod(int[] a, object[] b, IComparable[] c)
		{
			return false;
		}


		public IDictionary<string, string> test
		{
			get { throw new NotImplementedException(); }
		}
	}
}