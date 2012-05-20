using System;
using System.Collections.Generic;
using System.Linq;
using PlasmaTests.Sample;
using Plasma.Meta;

namespace PlasmaTests.Precompiler
{
//	public static class PerformRegistration
//	{
//		public static void Perform()
//		{
			
			
			

	// PlasmaTests.Sample, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null

	// iface - IMembershipProvider
public class ProxyMembershipProvider : Plasma.Proxy.ProxyBase<Sample.Proxy.IMembershipProvider>,  Sample.Proxy.IMembershipProvider
{
	public ProxyMembershipProvider(Sample.Proxy.IMembershipProvider originalObject) : base(originalObject)	{	}
	public virtual bool ValidateUser(string login, string password) { return Original.ValidateUser(login, password); }
	public virtual IEnumerable<string> ListUsers() { return Original.ListUsers(); }
	public virtual IList<string> ListUsers2() { return Original.ListUsers2(); }
	public virtual void AddUser(string login, string password) { Original.AddUser(login, password); }
	public virtual void DeleteUser(string login) { Original.DeleteUser(login); }
	public virtual Byte[] TestArray() { return Original.TestArray(); }
}
// iface - IMyServiceWithOptionalArguments
public class ProxyMyServiceWithOptionalArguments : Plasma.Proxy.ProxyBase<IMyServiceWithOptionalArguments>,  IMyServiceWithOptionalArguments
{
	public ProxyMyServiceWithOptionalArguments(IMyServiceWithOptionalArguments originalObject) : base(originalObject)	{	}
	public virtual IMyStorage Storage {  get { return Original.Storage; } }
	public virtual IMyWorker Worker {  get { return Original.Worker; } }
}
// iface - IMyService
public class ProxyMyService : Plasma.Proxy.ProxyBase<IMyService>,  IMyService
{
	public ProxyMyService(IMyService originalObject) : base(originalObject)	{	}
	public virtual int MyMethod() { return Original.MyMethod(); }
}
// iface - IMyPerformer
public class ProxyMyPerformer : Plasma.Proxy.ProxyBase<IMyPerformer>,  IMyPerformer
{
	public ProxyMyPerformer(IMyPerformer originalObject) : base(originalObject)	{	}
}
// iface - IMyService3
public class ProxyMyService3 : Plasma.Proxy.ProxyBase<IMyService3>,  IMyService3
{
	public ProxyMyService3(IMyService3 originalObject) : base(originalObject)	{	}
	public virtual bool MyMethod() { return Original.MyMethod(); }
}
// iface - IMyService4
public class ProxyMyService4 : Plasma.Proxy.ProxyBase<IMyService4>,  IMyService4
{
	public ProxyMyService4(IMyService4 originalObject) : base(originalObject)	{	}
	public virtual bool MyMethod() { return Original.MyMethod(); }
}
// iface - IMyWorker
public class ProxyMyWorker : Plasma.Proxy.ProxyBase<IMyWorker>,  IMyWorker
{
	public ProxyMyWorker(IMyWorker originalObject) : base(originalObject)	{	}
	public virtual int Test {  get { return Original.Test; } }
}
// iface - IMyService2
public class ProxyMyService2 : Plasma.Proxy.ProxyBase<IMyService2>,  IMyService2
{
	public ProxyMyService2(IMyService2 originalObject) : base(originalObject)	{	}
	public virtual void MyMethod2() { Original.MyMethod2(); }
	public virtual IMyService SubService {  get { return Original.SubService; } }
}
// iface - ISimpleDataForStubbing
public class ProxySimpleDataForStubbing : Plasma.Proxy.ProxyBase<Sample.Proxy.ISimpleDataForStubbing>,  Sample.Proxy.ISimpleDataForStubbing
{
	public ProxySimpleDataForStubbing(Sample.Proxy.ISimpleDataForStubbing originalObject) : base(originalObject)	{	}
	public virtual int Test {  get { return Original.Test; } set { Original.Test = value; } }
}
// iface - IComplexStubbing
public class ProxyComplexStubbing : Plasma.Proxy.ProxyBase<Sample.Proxy.IComplexStubbing>,  Sample.Proxy.IComplexStubbing
{
	public ProxyComplexStubbing(Sample.Proxy.IComplexStubbing originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
// iface - IComplexStubbingDerived
public class ProxyComplexStubbingDerived : Plasma.Proxy.ProxyBase<Sample.Proxy.IComplexStubbingDerived>,  Sample.Proxy.IComplexStubbingDerived
{
	public ProxyComplexStubbingDerived(Sample.Proxy.IComplexStubbingDerived originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
// iface - IComplexStubbingDerived2
public class ProxyComplexStubbingDerived2 : Plasma.Proxy.ProxyBase<Sample.Proxy.IComplexStubbingDerived2>,  Sample.Proxy.IComplexStubbingDerived2
{
	public ProxyComplexStubbingDerived2(Sample.Proxy.IComplexStubbingDerived2 originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
// iface - IComplexStubbingDerived3
public class ProxyComplexStubbingDerived3 : Plasma.Proxy.ProxyBase<Sample.Proxy.IComplexStubbingDerived3>,  Sample.Proxy.IComplexStubbingDerived3
{
	public ProxyComplexStubbingDerived3(Sample.Proxy.IComplexStubbingDerived3 originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
// iface - IMyStorage
public class ProxyMyStorage : Plasma.Proxy.ProxyBase<IMyStorage>,  IMyStorage
{
	public ProxyMyStorage(IMyStorage originalObject) : base(originalObject)	{	}
}
// iface - IMembershipProvider
public class NullMembershipProvider :  Sample.Proxy.IMembershipProvider
{
	public static readonly NullMembershipProvider Instance = new NullMembershipProvider();	public virtual bool ValidateUser(string login, string password) { return default(bool);
 }
	public virtual IEnumerable<string> ListUsers() { return NullEnumerable<string>.Instance;
 }
	public virtual IList<string> ListUsers2() { return NullList<string>.Instance;
 }
	public virtual void AddUser(string login, string password) {  }
	public virtual void DeleteUser(string login) {  }
	public virtual Byte[] TestArray() { return (byte[])Enumerable.Empty<byte>();
 }
}
// iface - IMyServiceWithOptionalArguments
public class NullMyServiceWithOptionalArguments :  IMyServiceWithOptionalArguments
{
	public static readonly NullMyServiceWithOptionalArguments Instance = new NullMyServiceWithOptionalArguments();	public virtual IMyStorage Storage {  get { return NullMyStorage.Instance;
 } }
	public virtual IMyWorker Worker {  get { return NullMyWorker.Instance;
 } }
}
// iface - IMyService
public class NullMyService :  IMyService
{
	public static readonly NullMyService Instance = new NullMyService();	public virtual int MyMethod() { return default(int);
 }
}
// iface - IMyPerformer
public class NullMyPerformer :  IMyPerformer
{
	public static readonly NullMyPerformer Instance = new NullMyPerformer();}
// iface - IMyService3
public class NullMyService3 :  IMyService3
{
	public static readonly NullMyService3 Instance = new NullMyService3();	public virtual bool MyMethod() { return default(bool);
 }
}
// iface - IMyService4
public class NullMyService4 :  IMyService4
{
	public static readonly NullMyService4 Instance = new NullMyService4();	public virtual bool MyMethod() { return default(bool);
 }
}
// iface - IMyWorker
public class NullMyWorker :  IMyWorker
{
	public static readonly NullMyWorker Instance = new NullMyWorker();	public virtual int Test {  get { return default(int);
 } }
}
// iface - IMyService2
public class NullMyService2 :  IMyService2
{
	public static readonly NullMyService2 Instance = new NullMyService2();	public virtual void MyMethod2() {  }
	public virtual IMyService SubService {  get { return NullMyService.Instance;
 } }
}
// iface - ISimpleDataForStubbing
public class NullSimpleDataForStubbing :  Sample.Proxy.ISimpleDataForStubbing
{
	public static readonly NullSimpleDataForStubbing Instance = new NullSimpleDataForStubbing();	public virtual int Test {  get { return default(int);
 } set {  } }
}
// iface - IComplexStubbing
public class NullComplexStubbing :  Sample.Proxy.IComplexStubbing
{
	public static readonly NullComplexStubbing Instance = new NullComplexStubbing();	public virtual int M(long p, string q) { return default(int);
 }
	public virtual event Action Event {  add {  } remove {  } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add {  } remove {  } }
	public virtual int Test1 {  get { return default(int);
 } set {  } }
	public virtual int Test2 {  get { return default(int);
 } }
	public virtual int Test3 {  set {  } }
	public virtual Action DelegateProperty {  get { return delegate { };
 } set {  } }
	public virtual string this [Guid g, int r] {  get { return string.Empty;
 } set {  } }
}
// iface - IComplexStubbingDerived
public class NullComplexStubbingDerived :  Sample.Proxy.IComplexStubbingDerived
{
	public static readonly NullComplexStubbingDerived Instance = new NullComplexStubbingDerived();	public virtual int M(long p, string q) { return default(int);
 }
	public virtual event Action Event {  add {  } remove {  } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add {  } remove {  } }
	public virtual int Test1 {  get { return default(int);
 } set {  } }
	public virtual int Test2 {  get { return default(int);
 } }
	public virtual int Test3 {  set {  } }
	public virtual Action DelegateProperty {  get { return delegate { };
 } set {  } }
	public virtual string this [Guid g, int r] {  get { return string.Empty;
 } set {  } }
}
// iface - IComplexStubbingDerived2
public class NullComplexStubbingDerived2 :  Sample.Proxy.IComplexStubbingDerived2
{
	public static readonly NullComplexStubbingDerived2 Instance = new NullComplexStubbingDerived2();	public virtual int M(long p, string q) { return default(int);
 }
	public virtual event Action Event {  add {  } remove {  } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add {  } remove {  } }
	public virtual int Test1 {  get { return default(int);
 } set {  } }
	public virtual int Test2 {  get { return default(int);
 } }
	public virtual int Test3 {  set {  } }
	public virtual Action DelegateProperty {  get { return delegate { };
 } set {  } }
	public virtual string this [Guid g, int r] {  get { return string.Empty;
 } set {  } }
}
// iface - IComplexStubbingDerived3
public class NullComplexStubbingDerived3 :  Sample.Proxy.IComplexStubbingDerived3
{
	public static readonly NullComplexStubbingDerived3 Instance = new NullComplexStubbingDerived3();	public virtual int M(long p, string q) { return default(int);
 }
	public virtual event Action Event {  add {  } remove {  } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add {  } remove {  } }
	public virtual int Test1 {  get { return default(int);
 } set {  } }
	public virtual int Test2 {  get { return default(int);
 } }
	public virtual int Test3 {  set {  } }
	public virtual Action DelegateProperty {  get { return delegate { };
 } set {  } }
	public virtual string this [Guid g, int r] {  get { return string.Empty;
 } set {  } }
}
// iface - IMyStorage
public class NullMyStorage :  IMyStorage
{
	public static readonly NullMyStorage Instance = new NullMyStorage();}
// iface - IEnumerable`1
public class NullEnumerable<T> :  IEnumerable<T>
{
	public static readonly NullEnumerable<T> Instance = new NullEnumerable<T>();	IEnumerator<T> IEnumerable<T>.GetEnumerator() { return NullEnumerator<T>.Instance;
 }
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return NullEnumerator.Instance;
 }
}
// iface - IList`1
public class NullList<T> :  IList<T>
{
	public static readonly NullList<T> Instance = new NullList<T>();	public virtual int IndexOf(T item) { return default(int);
 }
	public virtual void Insert(int index, T item) {  }
	public virtual void RemoveAt(int index) {  }
	public virtual void Add(T item) {  }
	public virtual void Clear() {  }
	public virtual bool Contains(T item) { return default(bool);
 }
	public virtual void CopyTo(T[] array, int arrayIndex) {  }
	public virtual bool Remove(T item) { return default(bool);
 }
	IEnumerator<T> IEnumerable<T>.GetEnumerator() { return NullEnumerator<T>.Instance;
 }
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return NullEnumerator.Instance;
 }
	public virtual T this [int index] {  get { return Null.Object<T>();
 } set {  } }
	public virtual int Count {  get { return default(int);
 } }
	public virtual bool IsReadOnly {  get { return default(bool);
 } }
}
// iface - IEnumerator`1
public class NullEnumerator<T> :  IEnumerator<T>
{
	public static readonly NullEnumerator<T> Instance = new NullEnumerator<T>();	public virtual void Dispose() {  }
	public virtual bool MoveNext() { return default(bool);
 }
	public virtual void Reset() {  }
	T IEnumerator<T>.Current {  get { return Null.Object<T>();
 } }
	object System.Collections.IEnumerator.Current {  get { return string.Empty;
 } }
}
// iface - IEnumerator
public class NullEnumerator :  System.Collections.IEnumerator
{
	public static readonly NullEnumerator Instance = new NullEnumerator();	public virtual bool MoveNext() { return default(bool);
 }
	public virtual void Reset() {  }
	public virtual object Current {  get { return string.Empty;
 } }
}

public static partial class PlasmaRegistration
{
	public static void Run()
	{


Plasma.Internal.TypeFactoryRegister.Add<DataLazyPropertyInjection>(c => new DataLazyPropertyInjection());
Plasma.Internal.TypeFactoryRegister.Add<DataFuncPropertyInjection>(c => new DataFuncPropertyInjection());
Plasma.Internal.TypeFactoryRegister.Add<DataLazyConstructorInjection>(c => new DataLazyConstructorInjection(new Lazy<IMyService>(c.Get<IMyService>)));
Plasma.Internal.TypeFactoryRegister.Add<DataFuncConstructorInjection>(c => new DataFuncConstructorInjection(c.Get<IMyService>));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithStructPro>(c => new MyServiceWithStructPro());
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithOptionalArguments>(c => new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.TryGet<IMyWorker>()));
Plasma.Internal.TypeFactoryRegister.Add<MySubGroup>(c => new MySubGroup(c.Get<Plasma.IPlasmaProvider>()));
Plasma.Internal.TypeFactoryRegister.Add<Sample.Proxy.SuggestedProxyMembershipProvider>(c => new Sample.Proxy.SuggestedProxyMembershipProvider(c.Get<Sample.Proxy.IMembershipProvider>()));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithSeveralCtors>(c => new MyServiceWithSeveralCtors(c.Get<IMyService>(), c.Get<IMyService>(), c.Get<IMyService>()));
Plasma.Internal.TypeFactoryRegister.Add<MyPerformer>(c => new MyPerformer());
Plasma.Internal.TypeFactoryRegister.Add<MyWorker>(c => new MyWorker());
Plasma.Internal.TypeFactoryRegister.Add<MyService3>(c => new MyService3(c.Get<IMyPerformer>()));
Plasma.Internal.TypeFactoryRegister.Add<MyService4>(c => new MyService4());
Plasma.Internal.TypeFactoryRegister.Add<Class1>(c => new Class1());
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithStruct>(c => new MyServiceWithStruct(default(Guid)));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithOptionalStruct>(c => new MyServiceWithOptionalStruct());
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithOptionalString>(c => new MyServiceWithOptionalString());
Plasma.Internal.TypeFactoryRegister.Add<MyService2>(c => new MyService2(c.Get<IMyService>()));
Plasma.Internal.TypeFactoryRegister.Add<MyServiceWithString>(c => new MyServiceWithString(null));
Plasma.Internal.TypeFactoryRegister.Add<MyService>(c => new MyService());
Plasma.Internal.TypeFactoryRegister.Add<MyInmemStorage>(c => new MyInmemStorage());
Plasma.Internal.TypeFactoryRegister.Add<MyPipeStorage>(c => new MyPipeStorage());
Plasma.Internal.TypeFactoryRegister.Add<MyFileStorage>(c => new MyFileStorage());
Plasma.Internal.TypeFactoryRegister.Add<MyNodeHost>(c => new MyNodeHost());
Plasma.Internal.TypeFactoryRegister.Add<MyObjectMan>(c => new MyObjectMan(c.Get<IMyStorage, MyPipeStorage>()));
Plasma.Internal.TypeFactoryRegister.Add<MyGenericMethod>(c => new MyGenericMethod());

// Property injectors optimization
Plasma.Internal.TypeAutoPlumberRegister.Register<DataLazyPropertyInjection>((c, x)=>{
	x.LazyService = new Lazy<IMyService>(c.Get<IMyService>);
});
Plasma.Internal.TypeAutoPlumberRegister.Register<DataFuncPropertyInjection>((c, x)=>{
	x.LazyService = c.Get<IMyService>;
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<DataLazyConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<DataFuncConstructorInjection>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStructPro>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalArguments>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MySubGroup>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<Sample.Proxy.SuggestedProxyMembershipProvider>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithSeveralCtors>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyPerformer>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyWorker>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService3>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService4>((c, x)=>{
	x.Performer = c.Get<IMyPerformer>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<Class1>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalStruct>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithOptionalString>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyService2>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyServiceWithString>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyService>((c, x)=>{
	x.Worker = c.Get<IMyWorker>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyInmemStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyPipeStorage>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyFileStorage>();
Plasma.Internal.TypeAutoPlumberRegister.Register<MyNodeHost>((c, x)=>{
	x.Storage = c.Get<IMyStorage, MyFileStorage>();
});
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyObjectMan>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyGenericRole<T>>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyGenericRoleNonePlumbing<T>>();
Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<MyGenericMethod>();

	}
}




//		}
//	}
}
