using System;

namespace TurboFacTests.Sample
{
	class DataLazyConstructorInjection
	{
		public static int Created;

		readonly Lazy<IMyService> _lazyService;

		public IMyService MyService
		{
			get { return _lazyService.Value; }
		}

		public DataLazyConstructorInjection(Lazy<IMyService> lazyService)
		{
			Created++;
			_lazyService = lazyService;
		}

	}

	class DataFuncConstructorInjection
	{
		public static int Created;

		readonly Func<IMyService> _lazyService;

		public IMyService MyService
		{
			get { return _lazyService(); }
		}

		public DataFuncConstructorInjection(Func<IMyService> lazyService)
		{
			Created++;
			_lazyService = lazyService;
		}

	}
}
