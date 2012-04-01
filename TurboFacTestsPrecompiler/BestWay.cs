﻿using System;
using System.Collections.Generic;
using System.Linq;
using TurboFac.Meta;

using TurboFacTests.Sample;

namespace TurboFacTests.Precompiler
{
	public static class BestWay
	{
		public static void Perform()
		{
			var c = TurboFac.TurboContainer.Root;
			c.Add<MyWorker>(new Lazy<MyWorker>());
			c.Add<DataLazyConstructorInjection>();
			c.Add<DataFuncConstructorInjection>();
			c.Add<DataLazyPropertyInjection>();
			c.Add<DataFuncPropertyInjection>();
			c.Add<MyPerformer>();
			c.Add<MyService>();
			c.Add<MyService2>();
			c.Add<MyService3>();
			c.Add<MyServiceWithOptionalArguments>();
			c.Add<MyServiceWithString>();
			c.Add<MyServiceWithStruct>();
			c.Add<MyServiceWithOptionalStruct>();
			c.Add<MyServiceWithOptionalString>();
			c.Add<MyServiceWithStructPro>();
			c.Add<MyServiceWithSeveralCtors>();
			c.Add<MySubGroup>();
		}
	}
}