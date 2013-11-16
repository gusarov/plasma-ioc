using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

#if NET3
using MyUtils;
#endif

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class DataLazyPropertyInjection
	{
		[Inject]
		public Lazy<IMyService> LazyService { get; set; }

		public static int Created;

		public DataLazyPropertyInjection()
		{
			Created++;
		}
	}

	[RegisterService]
	public class DataFuncPropertyInjection
	{
		[Inject]
		public Func<IMyService> LazyService { get; set; }

		public static int Created;

		public DataFuncPropertyInjection()
		{
			Created++;
		}
	}


}