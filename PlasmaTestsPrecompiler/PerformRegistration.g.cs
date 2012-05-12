using System;
using System.Collections.Generic;
using System.Linq;

using Plasma;

using PlasmaTests.Sample;

namespace PlasmaTests.Precompiler
{
//	public static class PerformRegistration
//	{
//		public static void Perform()
//		{
			
			
			
			
public static class PlasmaRegistration
{
	public static void Run()
	{


// PlasmaTests.Sample, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyWorker>(c => new PlasmaTests.Sample.MyWorker());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyInmemStorage>(c => new PlasmaTests.Sample.MyInmemStorage());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyPipeStorage>(c => new PlasmaTests.Sample.MyPipeStorage());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyFileStorage>(c => new PlasmaTests.Sample.MyFileStorage());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyNodeHost>(c => new PlasmaTests.Sample.MyNodeHost());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyObjectMan>(c => new PlasmaTests.Sample.MyObjectMan(c.Get<PlasmaTests.Sample.IMyStorage, MyPipeStorage>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithOptionalArguments>(c => new PlasmaTests.Sample.MyServiceWithOptionalArguments(c.Get<PlasmaTests.Sample.IMyStorage>(), c.TryGet<PlasmaTests.Sample.IMyWorker>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.Proxy.SuggestedProxyMembershipProvider>(c => new PlasmaTests.Sample.Proxy.SuggestedProxyMembershipProvider(c.Get<PlasmaTests.Sample.Proxy.IMembershipProvider>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyPerformer>(c => new PlasmaTests.Sample.MyPerformer());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.DataLazyConstructorInjection>(c => new PlasmaTests.Sample.DataLazyConstructorInjection(new Lazy<PlasmaTests.Sample.IMyService>(c.Get<IMyService>)));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.DataFuncConstructorInjection>(c => new PlasmaTests.Sample.DataFuncConstructorInjection(c.Get<PlasmaTests.Sample.IMyService>));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyService>(c => new PlasmaTests.Sample.MyService());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MySubGroup>(c => new PlasmaTests.Sample.MySubGroup(c.Get<Plasma.IPlasmaProvider>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithSeveralCtors>(c => new PlasmaTests.Sample.MyServiceWithSeveralCtors(c.Get<PlasmaTests.Sample.IMyService>(), c.Get<PlasmaTests.Sample.IMyService>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.DataLazyPropertyInjection>(c => new PlasmaTests.Sample.DataLazyPropertyInjection());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.DataFuncPropertyInjection>(c => new PlasmaTests.Sample.DataFuncPropertyInjection());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.Class1>(c => new PlasmaTests.Sample.Class1());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithStructPro>(c => new PlasmaTests.Sample.MyServiceWithStructPro());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithStruct>(c => new PlasmaTests.Sample.MyServiceWithStruct(default(System.Guid)));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithOptionalStruct>(c => new PlasmaTests.Sample.MyServiceWithOptionalStruct());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithOptionalString>(c => new PlasmaTests.Sample.MyServiceWithOptionalString());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyService3>(c => new PlasmaTests.Sample.MyService3(c.Get<PlasmaTests.Sample.IMyPerformer>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyService4>(c => new PlasmaTests.Sample.MyService4());
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyService2>(c => new PlasmaTests.Sample.MyService2(c.Get<PlasmaTests.Sample.IMyService>()));
Plasma.Internal.TypeFactoryRegister.Add<PlasmaTests.Sample.MyServiceWithString>(c => new PlasmaTests.Sample.MyServiceWithString(null));

// Property injectors optimization
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyWorker>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyInmemStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyPipeStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyFileStorage>();
Plasma.Internal.TypeAutoPlumberRegister.Register<PlasmaTests.Sample.MyNodeHost>((c, x)=>{
	x.Storage = c.Get<PlasmaTests.Sample.IMyStorage, MyFileStorage>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyObjectMan>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithOptionalArguments>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.Proxy.SuggestedProxyMembershipProvider>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyPerformer>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.DataLazyConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.DataFuncConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.Register<PlasmaTests.Sample.MyService>((c, x)=>{
	x.Worker = c.Get<PlasmaTests.Sample.IMyWorker>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MySubGroup>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithSeveralCtors>();
Plasma.Internal.TypeAutoPlumberRegister.Register<PlasmaTests.Sample.DataLazyPropertyInjection>((c, x)=>{
	x.LazyService = new Lazy<PlasmaTests.Sample.IMyService>(c.Get<IMyService>);
});
Plasma.Internal.TypeAutoPlumberRegister.Register<PlasmaTests.Sample.DataFuncPropertyInjection>((c, x)=>{
	x.LazyService = c.Get<PlasmaTests.Sample.IMyService>;
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.Class1>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithStructPro>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithOptionalStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithOptionalString>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyService3>();
Plasma.Internal.TypeAutoPlumberRegister.Register<PlasmaTests.Sample.MyService4>((c, x)=>{
	x.Performer = c.Get<PlasmaTests.Sample.IMyPerformer>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyService2>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<PlasmaTests.Sample.MyServiceWithString>();

	}
}


public class ProxyMyWorker : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.IMyWorker>, PlasmaTests.Sample.IMyWorker
{
	public ProxyMyWorker(PlasmaTests.Sample.IMyWorker originalObject) : base(originalObject)	{	}
	public virtual int Test {  get { return Original.Test; } }
}
public class ProxyMyServiceWithOptionalArguments : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.IMyServiceWithOptionalArguments>, PlasmaTests.Sample.IMyServiceWithOptionalArguments
{
	public ProxyMyServiceWithOptionalArguments(PlasmaTests.Sample.IMyServiceWithOptionalArguments originalObject) : base(originalObject)	{	}
	public virtual PlasmaTests.Sample.IMyStorage Storage {  get { return Original.Storage; } }
	public virtual PlasmaTests.Sample.IMyWorker Worker {  get { return Original.Worker; } }
}
public class ProxyMyService3 : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.IMyService3>, PlasmaTests.Sample.IMyService3
{
	public ProxyMyService3(PlasmaTests.Sample.IMyService3 originalObject) : base(originalObject)	{	}
	public virtual bool MyMethod() { return Original.MyMethod(); }
}
public class ProxyMyService4 : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.IMyService4>, PlasmaTests.Sample.IMyService4
{
	public ProxyMyService4(PlasmaTests.Sample.IMyService4 originalObject) : base(originalObject)	{	}
	public virtual bool MyMethod() { return Original.MyMethod(); }
}
public class ProxyMyService2 : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.IMyService2>, PlasmaTests.Sample.IMyService2
{
	public ProxyMyService2(PlasmaTests.Sample.IMyService2 originalObject) : base(originalObject)	{	}
	public virtual void MyMethod2() { Original.MyMethod2(); }
	public virtual PlasmaTests.Sample.IMyService SubService {  get { return Original.SubService; } }
}
public class ProxyMyService : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.IMyService>, PlasmaTests.Sample.IMyService
{
	public ProxyMyService(PlasmaTests.Sample.IMyService originalObject) : base(originalObject)	{	}
	public virtual int MyMethod() { return Original.MyMethod(); }
}
public class ProxyMembershipProvider : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.Proxy.IMembershipProvider>, PlasmaTests.Sample.Proxy.IMembershipProvider
{
	public ProxyMembershipProvider(PlasmaTests.Sample.Proxy.IMembershipProvider originalObject) : base(originalObject)	{	}
	public virtual bool ValidateUser(string login, string password) { return Original.ValidateUser(login, password); }
	public virtual System.Collections.Generic.IEnumerable<string> ListUsers() { return Original.ListUsers(); }
	public virtual void AddUser(string login, string password) { Original.AddUser(login, password); }
	public virtual void DeleteUser(string login) { Original.DeleteUser(login); }
}
public class ProxySimpleDataForStubbing : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.Proxy.ISimpleDataForStubbing>, PlasmaTests.Sample.Proxy.ISimpleDataForStubbing
{
	public ProxySimpleDataForStubbing(PlasmaTests.Sample.Proxy.ISimpleDataForStubbing originalObject) : base(originalObject)	{	}
	public virtual int Test {  get { return Original.Test; } set { Original.Test = value; } }
}
public class ProxyComplexStubbing : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.Proxy.IComplexStubbing>, PlasmaTests.Sample.Proxy.IComplexStubbing
{
	public ProxyComplexStubbing(PlasmaTests.Sample.Proxy.IComplexStubbing originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event System.Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual System.Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
public class ProxyComplexStubbingDerived : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.Proxy.IComplexStubbingDerived>, PlasmaTests.Sample.Proxy.IComplexStubbingDerived
{
	public ProxyComplexStubbingDerived(PlasmaTests.Sample.Proxy.IComplexStubbingDerived originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event System.Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual System.Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
public class ProxyComplexStubbingDerived2 : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.Proxy.IComplexStubbingDerived2>, PlasmaTests.Sample.Proxy.IComplexStubbingDerived2
{
	public ProxyComplexStubbingDerived2(PlasmaTests.Sample.Proxy.IComplexStubbingDerived2 originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event System.Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual System.Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
public class ProxyComplexStubbingDerived3 : Plasma.Proxy.ProxyBase<PlasmaTests.Sample.Proxy.IComplexStubbingDerived3>, PlasmaTests.Sample.Proxy.IComplexStubbingDerived3
{
	public ProxyComplexStubbingDerived3(PlasmaTests.Sample.Proxy.IComplexStubbingDerived3 originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event System.Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual System.Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}

//		}
//	}
}
