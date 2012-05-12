using System;
using System.Collections.Generic;
using System.Linq;

using Plasma;

using PlasmaTests.Sample;

namespace PlasmaTests.Precompiler
{
	public static class PerformRegistration
	{
		public static void Perform()
		{
			
			
			
			
// PlasmaTests.Sample, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
Plasma.Internal.TypeFactoryRegister.Add<MyWorker>(c => new MyWorker());
Plasma.Internal.TypeFactoryRegister.Add<MyInmemStorage>(c => new MyInmemStorage());
Plasma.Internal.TypeFactoryRegister.Add<MyPipeStorage>(c => new MyPipeStorage());
Plasma.Internal.TypeFactoryRegister.Add<MyFileStorage>(c => new MyFileStorage());
Plasma.Internal.TypeFactoryRegister.Add<MyNodeHost>(c => new MyNodeHost());
Plasma.Internal.TypeFactoryRegister.Add<MyObjectMan>(c => new MyObjectMan(c.Get<IMyStorage, MyPipeStorage>()));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithOptionalArguments>(c => new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.TryGet<IMyWorker>()));
Plasma.Internal.TypeFactoryRegister.Add<MyPerformer>(c => new MyPerformer());
Plasma.Internal.TypeFactoryRegister.Add<DataLazyConstructorInjection>(c => new DataLazyConstructorInjection(new Lazy<IMyService>(c.Get<IMyService>)));
Plasma.Internal.TypeFactoryRegister.Add<DataFuncConstructorInjection>(c => new DataFuncConstructorInjection(c.Get<IMyService>));
Plasma.Internal.TypeFactoryRegister.Add<MyService>(c => new MyService());
Plasma.Internal.TypeFactoryRegister.Add<MySubGroup>(c => new MySubGroup(c.Get<IPlasmaProvider>()));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithSeveralCtors>(c => new MyServiceWithSeveralCtors(c.Get<IMyService>(), c.Get<IMyService>()));
Plasma.Internal.TypeFactoryRegister.Add<DataLazyPropertyInjection>(c => new DataLazyPropertyInjection());
Plasma.Internal.TypeFactoryRegister.Add<DataFuncPropertyInjection>(c => new DataFuncPropertyInjection());
Plasma.Internal.TypeFactoryRegister.Add<Class1>(c => new Class1());
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithStructPro>(c => new MyServiceWithStructPro());
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithStruct>(c => new MyServiceWithStruct(default(Guid)));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithOptionalStruct>(c => new MyServiceWithOptionalStruct());
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithOptionalString>(c => new MyServiceWithOptionalString());
Plasma.Internal.TypeFactoryRegister.Add<MyService3>(c => new MyService3(c.Get<IMyPerformer>()));
Plasma.Internal.TypeFactoryRegister.Add<MyService4>(c => new MyService4());
Plasma.Internal.TypeFactoryRegister.Add<MyService2>(c => new MyService2(c.Get<IMyService>()));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithString>(c => new MyServiceWithString(null));

// Property injectors optimization
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyInmemStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyPipeStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyFileStorage>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyNodeHost>((c, x)=>{
	x.Storage = c.Get<IMyStorage, MyFileStorage>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyObjectMan>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyPerformer>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<DataLazyConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<DataFuncConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService>((c, x)=>{
	x.Worker = c.Get<IMyWorker>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MySubGroup>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithSeveralCtors>();
Plasma.Internal.TypeAutoPlumberRegister.Register<DataLazyPropertyInjection>((c, x)=>{
	x.LazyService = new Lazy<IMyService>(c.Get<IMyService>);
});
Plasma.Internal.TypeAutoPlumberRegister.Register<DataFuncPropertyInjection>((c, x)=>{
	x.LazyService = c.Get<IMyService>;
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<Class1>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStructPro>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalString>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService4>((c, x)=>{
	x.Performer = c.Get<IMyPerformer>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService2>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithString>();

		}
	}
}
