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
			
			
			
			
c.Add<IMyPerformer>(()=>new MyPerformer());
c.Add<IMyStorage>(()=>new MyInmemStorage());
c.Add<MyNodeHost>(()=>new MyNodeHost());
c.Add<MyObjectMan>(()=>new MyObjectMan(c.Get<IMyStorage>()));
c.Add<IMyService3>(()=>new MyService3(c.Get<IMyPerformer>()));
c.Add<IMyService4>(()=>new MyService4());
c.Add<MyServiceWithStructPro>(()=>new MyServiceWithStructPro());
c.Add<IMyWorker>(()=>new MyWorker());
c.Add<IMyWorker>(()=>new MyWorker());
c.Add<MySubGroup>(()=>new MySubGroup(c.Get<ITurboProvider>()));
c.Add<IMyServiceWithOptionalArguments>(()=>new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.Get<IMyWorker>()));
c.Add<IMyServiceWithOptionalArguments>(()=>new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.Get<IMyWorker>()));
c.Add<DataLazyConstructorInjection>(()=>new DataLazyConstructorInjection(new Lazy<IMyService>(c.Get<IMyService>)));
c.Add<DataFuncConstructorInjection>(()=>new DataFuncConstructorInjection(c.Get<IMyService>));
c.Add<IMyService>(()=>new MyService());
c.Add<DataLazyPropertyInjection>(()=>new DataLazyPropertyInjection());
c.Add<DataFuncPropertyInjection>(()=>new DataFuncPropertyInjection());
c.Add<MyServiceWithStruct>(()=>new MyServiceWithStruct(default(Guid)));
c.Add<MyServiceWithOptionalStruct>(()=>new MyServiceWithOptionalStruct());
c.Add<MyServiceWithOptionalString>(()=>new MyServiceWithOptionalString());
c.Add<IMyService3>(()=>new MyService3(c.Get<IMyPerformer>()));
c.Add<IMyService4>(()=>new MyService4());
c.Add<MyServiceWithSeveralCtors>(()=>new MyServiceWithSeveralCtors(c.Get<IMyService>(), c.Get<IMyService>()));
c.Add<MyServiceWithString>(()=>new MyServiceWithString(null));
c.Add<MyService2>(()=>new MyService2(c.Get<IMyService>()));
c.Add<IMyService>(()=>new MyService());

// Property injectors optimization
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyPerformer>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<MyNodeHost>((c_, x_)=>{
	x_.Storage = c.Get<IMyStorage>();
});
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyObjectMan>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStructPro>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MySubGroup>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<DataLazyConstructorInjection>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<DataFuncConstructorInjection>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<DataLazyPropertyInjection>((c_, x_)=>{
	x_.LazyService = new Lazy<IMyService>(c.Get<IMyService>);
});
TurboFac.Internal.TypeAutoPlumberRegister.Register<DataFuncPropertyInjection>((c_, x_)=>{
	x_.LazyService = c.Get<IMyService>;
});
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStruct>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalStruct>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalString>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<MyService4>((c_, x_)=>{
	x_.Performer = c.Get<IMyPerformer>();
});
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithSeveralCtors>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithString>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyService2>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<MyService>((c_, x_)=>{
	x_.Worker = c.Get<IMyWorker>();
});
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyInmemStorage>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<MyService4>((c_, x_)=>{
	x_.Performer = c.Get<IMyPerformer>();
});
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();
TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
TurboFac.Internal.TypeAutoPlumberRegister.Register<MyService>((c_, x_)=>{
	x_.Worker = c.Get<IMyWorker>();
});

		}
	}
}
