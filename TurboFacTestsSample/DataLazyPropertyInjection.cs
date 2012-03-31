using System;

using TurboFac;

#if NET3
using MyUtils;
#endif

namespace TurboFacTests.Sample
{
	[TurboReg]
	public class DataLazyPropertyInjection
	{
		public Lazy<IMyService> LazyService { get; set; }

		public static int Created;

		public DataLazyPropertyInjection()
		{
			Created++;
		}
	}

	[TurboReg]
	public class DataFuncPropertyInjection
	{
		public Func<IMyService> LazyService { get; set; }

		public static int Created;

		public DataFuncPropertyInjection()
		{
			Created++;
		}
	}


}