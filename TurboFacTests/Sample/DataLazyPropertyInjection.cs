using System;

#if NET3
using MyUtils;
#endif

namespace TurboFacTests.Sample
{
	class DataLazyPropertyInjection
	{
		public Lazy<IMyService> LazyService { get; set; }

		public static int Created;

		public DataLazyPropertyInjection()
		{
			Created++;
		}
	}
}