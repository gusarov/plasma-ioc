using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlasmaTests.Sample
{
	public class MyGenericRole<T>
	{
		public void Test()
		{

		}

		public void Test<T>()
		{

		}

		public MyGenericRole<T> MyGenericProperty { get; set; }
	}

	public class MyGenericRoleNonePlumbing<T>
	{
		public void Test()
		{

		}

		public void Test<T>()
		{

		}
	}

	public class MyGenericMethod
	{
		public MyGenericRole<MyGenericMethod> MyGenericProperty { get; set; }

		public void Test<T1>() {}

		public T1 Test<T1, T2>(Func<T1> p1, Action<T1> p2, T2 p3)
		{
			return default(T1);
		}
	}


}
