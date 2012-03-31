using System;
using System.Collections.Generic;
using System.Linq;
using TurboFac;
using TurboFac.Meta;
using TurboFacTests.Sample;
	
namespace TurboFacTests.Precompiler
{
	public static class PerformRegistration
	{
		public static void Perform(ITurboContainer c)
		{
			
			
			
			// kxujuiis, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TurboFacTests.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
c.Add<IMyPerformer>(()=>new MyPerformer());
c.Add<MyServiceWithStructPro>(()=>new MyServiceWithStructPro());
c.Add<IMyWorker>(()=>new MyWorker());
c.Add<MySubGroup>(()=>new MySubGroup(c.Get<ITurboProvider>()));
c.Add<IMyServiceWithOptionalArguments>(()=>new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.Get<IMyWorker>()));
c.Add<DataLazyConstructorInjection>(()=>new DataLazyConstructorInjection(new Lazy<IMyService>(c.Get<IMyService>)));
c.Add<DataFuncConstructorInjection>(()=>new DataFuncConstructorInjection(c.Get<IMyService>));
c.Add<DataLazyPropertyInjection>(()=>new DataLazyPropertyInjection());
c.Add<DataFuncPropertyInjection>(()=>new DataFuncPropertyInjection());
c.Add<MyServiceWithStruct>(()=>new MyServiceWithStruct(default(Guid)));
c.Add<MyServiceWithOptionalStruct>(()=>new MyServiceWithOptionalStruct());
c.Add<MyServiceWithOptionalString>(()=>new MyServiceWithOptionalString());
c.Add<IMyService3>(()=>new MyService3(c.Get<IMyPerformer>()));
c.Add<MyServiceWithSeveralCtors>(()=>new MyServiceWithSeveralCtors(c.Get<IMyService>(), c.Get<IMyService>()));
c.Add<MyServiceWithString>(()=>new MyServiceWithString(null));
c.Add<MyService2>(()=>new MyService2(c.Get<IMyService>()));
c.Add<IMyService>(()=>new MyService());
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyPerformer>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStructPro>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MySubGroup>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<DataLazyConstructorInjection>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<DataFuncConstructorInjection>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<DataLazyPropertyInjection>(x=>{
var item = (DataLazyPropertyInjection)x;
item.LazyService = c.Get<Lazy<IMyService>>();

				});
TurboFac.Internal.TypeAutoPlumberRegister.Register<DataFuncPropertyInjection>(x=>{
var item = (DataFuncPropertyInjection)x;
item.LazyService = c.Get<Func<IMyService>>();

				});
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStruct>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalStruct>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalString>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithSeveralCtors>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithString>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyService2>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<MyService>(x=>{
var item = (MyService)x;
item.Worker = c.Get<IMyWorker>();

				});

		}
	}
}
