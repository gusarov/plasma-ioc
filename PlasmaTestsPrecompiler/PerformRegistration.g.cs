using System;
using System.Collections.Generic;
using System.Linq;

using Plasma;

using PlasmaTests.Sample;

namespace PlasmaTests.Precompiler
{
	public static class PerformRegistration
	{
		public static void Perform(IPlasmaContainer c)
		{
			
			
			
			
c.Add<DataLazyConstructorInjection>(()=>new DataLazyConstructorInjection(new Lazy<IMyService>(c.Get<IMyService>)));
c.Add<DataFuncConstructorInjection>(()=>new DataFuncConstructorInjection(c.Get<IMyService>));
c.Add<IMyService3>(()=>new MyService3(c.Get<IMyPerformer>()));
c.Add<IMyService3>(()=>new MyService3(c.Get<IMyPerformer>()));
c.Add<IMyService4>(()=>new MyService4());
c.Add<IMyService4>(()=>new MyService4());
c.Add<IMyService>(()=>new MyService());
c.Add<MyService2>(()=>new MyService2(c.Get<IMyService>()));
c.Add<IMyServiceWithOptionalArguments>(()=>new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.Get<IMyWorker>()));
c.Add<IMyServiceWithOptionalArguments>(()=>new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.Get<IMyWorker>()));
c.Add<MyServiceWithStructPro>(()=>new MyServiceWithStructPro());
c.Add<IMyStorage>(()=>new MyInmemStorage());
c.Add<MyNodeHost>(()=>new MyNodeHost());
c.Add<MyObjectMan>(()=>new MyObjectMan(c.Get<IMyStorage>()));
c.Add<MyServiceWithSeveralCtors>(()=>new MyServiceWithSeveralCtors(c.Get<IMyService>(), c.Get<IMyService>()));
c.Add<DataLazyPropertyInjection>(()=>new DataLazyPropertyInjection());
c.Add<DataFuncPropertyInjection>(()=>new DataFuncPropertyInjection());
c.Add<MyServiceWithStruct>(()=>new MyServiceWithStruct(default(Guid)));
c.Add<MyServiceWithOptionalStruct>(()=>new MyServiceWithOptionalStruct());
c.Add<MyServiceWithOptionalString>(()=>new MyServiceWithOptionalString());
c.Add<IMyWorker>(()=>new MyWorker());
c.Add<MyServiceWithString>(()=>new MyServiceWithString(null));
c.Add<IMyService>(()=>new MyService());
c.Add<IMyWorker>(()=>new MyWorker());
c.Add<MySubGroup>(()=>new MySubGroup(c.Get<IPlasmaProvider>()));
c.Add<IMyPerformer>(()=>new MyPerformer());

// Property injectors optimization
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<DataLazyConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<DataFuncConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService4>((c_, x_)=>{
	x_.Performer = c.Get<IMyPerformer>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService2>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStructPro>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyNodeHost>((c_, x_)=>{
	x_.Storage = c.Get<IMyStorage>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyObjectMan>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithSeveralCtors>();
Plasma.Internal.TypeAutoPlumberRegister.Register<DataLazyPropertyInjection>((c_, x_)=>{
	x_.LazyService = new Lazy<IMyService>(c.Get<IMyService>);
});
Plasma.Internal.TypeAutoPlumberRegister.Register<DataFuncPropertyInjection>((c_, x_)=>{
	x_.LazyService = c.Get<IMyService>;
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalString>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithString>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService>((c_, x_)=>{
	x_.Worker = c.Get<IMyWorker>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MySubGroup>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyPerformer>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService4>((c_, x_)=>{
	x_.Performer = c.Get<IMyPerformer>();
});
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService>((c_, x_)=>{
	x_.Worker = c.Get<IMyWorker>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyInmemStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();

		}
	}
}
