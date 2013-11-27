using System;
using System.Collections.Generic;
using System.Linq;
using PlasmaTests.Sample;
using Plasma.Meta;
using Plasma.Internal;

using PlasmaTests;
using PlasmaTests.Sample;
using PlasmaTests.Sample.Proxy;

namespace PlasmaTests.Precompiler
{
	
	/* + static object ref2 = typeof(Spring.Objects.Factory.Config.ObjectFactoryCreatingFactoryObject); */

	
	
	
	

	
	

	// Class1
// ISession
// SessionFactory
// Session
// GenericPerformer
// HibernateCrudDao<T>
// IMyService4
// IMyServiceComplex
// MyGenericRole<T>
// MyGenericRoleNonePlumbing<T>
// MyGenericMethod
// MyService4
// MyService5
// MyServiceWithAutomaticSetterOnlyInjection
// IMyServiceWithMatchedIface
// MyServiceWithMatchedIface
// IMyUniqueIFace
// MyServiceWithUniqueIFace
// IPrivateIFace
// PrivateInner
// ISimpleDataForStubbing
// IComplexStubbing
// IComplexStubbingDerived
// IComplexStubbingDerived2
// IComplexStubbingDerived3
// SuggestedProxyMembershipProvider
// DataLazyConstructorInjection
// DataFuncConstructorInjection
// DataLazyPropertyInjection
// DataFuncPropertyInjection
// IMembershipProvider
// IMyPerformer
// IMyService
// IMyService2
// IMyService3
// IMyStorage
// MyInmemStorage
// MyPipeStorage
// MyFileStorage
// MyNodeHost
// MyObjectMan
// IMyWorker
// MyPerformer
// MyService
// MyService6WithoutInterface
// MyService7Dependency
// MyBadServiceDep
// MyBadService
// MyService2
// MyService3
// IMyServiceWithOptionalArguments
// MyServiceWithOptionalArguments
// MyServiceWithString
// MyServiceWithStruct
// MyServiceWithOptionalStruct
// MyServiceWithOptionalString
// MyServiceWithStructPro
// MyServiceWithSeveralCtors
// MySubGroup
// MyWorker

	// Analyze...
// Miner request for HibernateCrudDao<IMyService>
// Miner request for IMembershipProvider
// Miner request for IMyService
// Miner request for IMyService
// Miner request for IMyStorage
// Miner request for MyService7Dependency
// Miner request for MyBadServiceDep
// Miner request for IMyService
// Miner request for IMyPerformer
// Miner request for IMyStorage
// Miner request for IMyWorker
// Miner request for IMyService
// Miner request for IMyService
// Miner request for Plasma.IPlasmaProvider
// Miner request for HibernateCrudDao<IMyService2>
// Miner request for HibernateCrudDao<IMyService3>
// Miner request for IMyPerformer
// Miner request for IMyService
// Miner request for IMyService
// Miner request for IMyService
// Miner request for IMyStorage
// Miner request for IMyWorker
// New requests: HibernateCrudDao<IMyService>, Plasma.IPlasmaProvider, HibernateCrudDao<IMyService2>, HibernateCrudDao<IMyService3>
// Miner request for ISession
// Miner request for ISession
// Miner request for ISession
// New requests: Plasma.PlasmaContainer
// New requests: 
// Analyze completed
// iface - ISession
public class ProxySession : Plasma.Proxy.ProxyBase<ISession>,  ISession
{
	public ProxySession(ISession originalObject) : base(originalObject)	{	}
	public virtual string Config {  get { return Original.Config; } }
}
// iface - IMyService4
public class ProxyMyService4 : Plasma.Proxy.ProxyBase<IMyService4>,  IMyService4
{
	public ProxyMyService4(IMyService4 originalObject) : base(originalObject)	{	}
	public virtual bool MyMethod() { return Original.MyMethod(); }
}
// iface - IMyServiceComplex
public class ProxyMyServiceComplex : Plasma.Proxy.ProxyBase<IMyServiceComplex>,  IMyServiceComplex
{
	public ProxyMyServiceComplex(IMyServiceComplex originalObject) : base(originalObject)	{	}
	bool IMyServiceComplex.MyMethod(int a, out int b) { return Original.MyMethod(a, out b); }
	bool IMyServiceComplex.MyMethod(int[] a, object[] b, IComparable[] c) { return Original.MyMethod(a, b, c); }
}
// iface - IMyServiceWithMatchedIface
public class ProxyMyServiceWithMatchedIface : Plasma.Proxy.ProxyBase<IMyServiceWithMatchedIface>,  IMyServiceWithMatchedIface
{
	public ProxyMyServiceWithMatchedIface(IMyServiceWithMatchedIface originalObject) : base(originalObject)	{	}
}
// iface - IMyUniqueIFace
public class ProxyMyUniqueIFace : Plasma.Proxy.ProxyBase<IMyUniqueIFace>,  IMyUniqueIFace
{
	public ProxyMyUniqueIFace(IMyUniqueIFace originalObject) : base(originalObject)	{	}
}
// iface - IPrivateIFace
public class ProxyPrivateIFace : Plasma.Proxy.ProxyBase<IPrivateIFace>,  IPrivateIFace
{
	public ProxyPrivateIFace(IPrivateIFace originalObject) : base(originalObject)	{	}
}
// iface - ISimpleDataForStubbing
public class ProxySimpleDataForStubbing : Plasma.Proxy.ProxyBase<ISimpleDataForStubbing>,  ISimpleDataForStubbing
{
	public ProxySimpleDataForStubbing(ISimpleDataForStubbing originalObject) : base(originalObject)	{	}
	public virtual int Test {  get { return Original.Test; } set { Original.Test = value; } }
}
// iface - IComplexStubbing
public class ProxyComplexStubbing : Plasma.Proxy.ProxyBase<IComplexStubbing>,  IComplexStubbing
{
	public ProxyComplexStubbing(IComplexStubbing originalObject) : base(originalObject)	{	}
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
public class ProxyComplexStubbingDerived : Plasma.Proxy.ProxyBase<IComplexStubbingDerived>,  IComplexStubbingDerived
{
	public ProxyComplexStubbingDerived(IComplexStubbingDerived originalObject) : base(originalObject)	{	}
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
public class ProxyComplexStubbingDerived2 : Plasma.Proxy.ProxyBase<IComplexStubbingDerived2>,  IComplexStubbingDerived2
{
	public ProxyComplexStubbingDerived2(IComplexStubbingDerived2 originalObject) : base(originalObject)	{	}
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
public class ProxyComplexStubbingDerived3 : Plasma.Proxy.ProxyBase<IComplexStubbingDerived3>,  IComplexStubbingDerived3
{
	public ProxyComplexStubbingDerived3(IComplexStubbingDerived3 originalObject) : base(originalObject)	{	}
	public virtual int M(long p, string q) { return Original.M(p, q); }
	public virtual event Action Event {  add { Original.Event += value; } remove { Original.Event -= value; } }
	public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged {  add { Original.PropertyChanged += value; } remove { Original.PropertyChanged -= value; } }
	public virtual int Test1 {  get { return Original.Test1; } set { Original.Test1 = value; } }
	public virtual int Test2 {  get { return Original.Test2; } }
	public virtual int Test3 {  set { Original.Test3 = value; } }
	public virtual Action DelegateProperty {  get { return Original.DelegateProperty; } set { Original.DelegateProperty = value; } }
	public virtual string this [Guid g, int r] {  get { return Original[g, r]; } set { Original[g, r] = value; } }
}
// iface - IMembershipProvider
public class ProxyMembershipProvider : Plasma.Proxy.ProxyBase<IMembershipProvider>,  IMembershipProvider
{
	public ProxyMembershipProvider(IMembershipProvider originalObject) : base(originalObject)	{	}
	public virtual bool ValidateUser(string login, string password) { return Original.ValidateUser(login, password); }
	public virtual IEnumerable<string> ListUsers() { return Original.ListUsers(); }
	public virtual IList<string> ListUsers2() { return Original.ListUsers2(); }
	public virtual void AddUser(string login, string password) { Original.AddUser(login, password); }
	public virtual void DeleteUser(string login) { Original.DeleteUser(login); }
	public virtual byte[] TestArray() { return Original.TestArray(); }
}
// iface - IMyPerformer
public class ProxyMyPerformer : Plasma.Proxy.ProxyBase<IMyPerformer>,  IMyPerformer
{
	public ProxyMyPerformer(IMyPerformer originalObject) : base(originalObject)	{	}
}
// iface - IMyService
public class ProxyMyService : Plasma.Proxy.ProxyBase<IMyService>,  IMyService
{
	public ProxyMyService(IMyService originalObject) : base(originalObject)	{	}
	public virtual int MyMethod() { return Original.MyMethod(); }
}
// iface - IMyService2
public class ProxyMyService2 : Plasma.Proxy.ProxyBase<IMyService2>,  IMyService2
{
	public ProxyMyService2(IMyService2 originalObject) : base(originalObject)	{	}
	public virtual void MyMethod2() { Original.MyMethod2(); }
	public virtual IMyService SubService {  get { return Original.SubService; } }
}
// iface - IMyService3
public class ProxyMyService3 : Plasma.Proxy.ProxyBase<IMyService3>,  IMyService3
{
	public ProxyMyService3(IMyService3 originalObject) : base(originalObject)	{	}
	public virtual bool MyMethod() { return Original.MyMethod(); }
}
// iface - IMyStorage
public class ProxyMyStorage : Plasma.Proxy.ProxyBase<IMyStorage>,  IMyStorage
{
	public ProxyMyStorage(IMyStorage originalObject) : base(originalObject)	{	}
}
// iface - IMyWorker
public class ProxyMyWorker : Plasma.Proxy.ProxyBase<IMyWorker>,  IMyWorker
{
	public ProxyMyWorker(IMyWorker originalObject) : base(originalObject)	{	}
	public virtual int Test {  get { return Original.Test; } }
}
// iface - IMyServiceWithOptionalArguments
public class ProxyMyServiceWithOptionalArguments : Plasma.Proxy.ProxyBase<IMyServiceWithOptionalArguments>,  IMyServiceWithOptionalArguments
{
	public ProxyMyServiceWithOptionalArguments(IMyServiceWithOptionalArguments originalObject) : base(originalObject)	{	}
	public virtual IMyStorage Storage {  get { return Original.Storage; } }
	public virtual IMyWorker Worker {  get { return Original.Worker; } }
}
// iface - ISession
public class NullSession :  ISession
{
	public static readonly NullSession Instance = new NullSession();	public virtual string Config {  get { return string.Empty;
 } }
}
// iface - IMyService4
public class NullMyService4 :  IMyService4
{
	public static readonly NullMyService4 Instance = new NullMyService4();	public virtual bool MyMethod() { return default(bool);
 }
}
// iface - IMyServiceComplex
public class NullMyServiceComplex :  IMyServiceComplex
{
	public static readonly NullMyServiceComplex Instance = new NullMyServiceComplex();	bool IMyServiceComplex.MyMethod(int a, out int b) { b = default(int);
return default(bool);
 }
	bool IMyServiceComplex.MyMethod(int[] a, object[] b, IComparable[] c) { return default(bool);
 }
}
// iface - IMyServiceWithMatchedIface
public class NullMyServiceWithMatchedIface :  IMyServiceWithMatchedIface
{
	public static readonly NullMyServiceWithMatchedIface Instance = new NullMyServiceWithMatchedIface();}
// iface - IMyUniqueIFace
public class NullMyUniqueIFace :  IMyUniqueIFace
{
	public static readonly NullMyUniqueIFace Instance = new NullMyUniqueIFace();}
// iface - IPrivateIFace
public class NullPrivateIFace :  IPrivateIFace
{
	public static readonly NullPrivateIFace Instance = new NullPrivateIFace();}
// iface - ISimpleDataForStubbing
public class NullSimpleDataForStubbing :  ISimpleDataForStubbing
{
	public static readonly NullSimpleDataForStubbing Instance = new NullSimpleDataForStubbing();	public virtual int Test {  get { return default(int);
 } set {  } }
}
// iface - IComplexStubbing
public class NullComplexStubbing :  IComplexStubbing
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
public class NullComplexStubbingDerived :  IComplexStubbingDerived
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
public class NullComplexStubbingDerived2 :  IComplexStubbingDerived2
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
public class NullComplexStubbingDerived3 :  IComplexStubbingDerived3
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
// iface - IMembershipProvider
public class NullMembershipProvider :  IMembershipProvider
{
	public static readonly NullMembershipProvider Instance = new NullMembershipProvider();	public virtual bool ValidateUser(string login, string password) { return default(bool);
 }
	public virtual IEnumerable<string> ListUsers() { return NullEnumerable<string>.Instance;
 }
	public virtual IList<string> ListUsers2() { return NullList<string>.Instance;
 }
	public virtual void AddUser(string login, string password) {  }
	public virtual void DeleteUser(string login) {  }
	public virtual byte[] TestArray() { return (byte[])Enumerable.Empty<byte>();
 }
}
// iface - IMyPerformer
public class NullMyPerformer :  IMyPerformer
{
	public static readonly NullMyPerformer Instance = new NullMyPerformer();}
// iface - IMyService
public class NullMyService :  IMyService
{
	public static readonly NullMyService Instance = new NullMyService();	public virtual int MyMethod() { return default(int);
 }
}
// iface - IMyService2
public class NullMyService2 :  IMyService2
{
	public static readonly NullMyService2 Instance = new NullMyService2();	public virtual void MyMethod2() {  }
	public virtual IMyService SubService {  get { return NullMyService.Instance;
 } }
}
// iface - IMyService3
public class NullMyService3 :  IMyService3
{
	public static readonly NullMyService3 Instance = new NullMyService3();	public virtual bool MyMethod() { return default(bool);
 }
}
// iface - IMyStorage
public class NullMyStorage :  IMyStorage
{
	public static readonly NullMyStorage Instance = new NullMyStorage();}
// iface - IMyWorker
public class NullMyWorker :  IMyWorker
{
	public static readonly NullMyWorker Instance = new NullMyWorker();	public virtual int Test {  get { return default(int);
 } }
}
// iface - IMyServiceWithOptionalArguments
public class NullMyServiceWithOptionalArguments :  IMyServiceWithOptionalArguments
{
	public static readonly NullMyServiceWithOptionalArguments Instance = new NullMyServiceWithOptionalArguments();	public virtual IMyStorage Storage {  get { return NullMyStorage.Instance;
 } }
	public virtual IMyWorker Worker {  get { return NullMyWorker.Instance;
 } }
}
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
	object System.Collections.IEnumerator.Current {  get { return Null.Object<object>();
 } }
}
// iface - IEnumerator
public class NullEnumerator :  System.Collections.IEnumerator
{
	public static readonly NullEnumerator Instance = new NullEnumerator();	public virtual bool MoveNext() { return default(bool);
 }
	public virtual void Reset() {  }
	public virtual object Current {  get { return Null.Object<object>();
 } }
}

public static partial class PlasmaRegistration
{
	static volatile bool _executed;

	public static void Run(Plasma.ReflectionPermission? reflectionPermission = null)
	{
		if (_executed)
		{
			return;
		}
		_executed = true;
		Plasma.PlasmaContainer.DefaultReflectionPermission = reflectionPermission ?? Plasma.ReflectionPermission.Throw;



// Factory 
TypeFactoryRegister.Add<Class1>(c => new Class1());
TypeFactoryRegister.Add<SessionFactory>(c => new SessionFactory());
TypeFactoryRegister.Add<Session>(c => c.Get<SessionFactory>().Create());
TypeFactoryRegister.Add<GenericPerformer>(c => new GenericPerformer(c.Get<HibernateCrudDao<IMyService>>()));
TypeFactoryRegister.Add<MyGenericMethod>(c => new MyGenericMethod());
TypeFactoryRegister.Add<MyService4>(c => new MyService4());
TypeFactoryRegister.Add<MyService5>(c => new MyService5());
TypeFactoryRegister.Add<MyServiceWithAutomaticSetterOnlyInjection>(c => new MyServiceWithAutomaticSetterOnlyInjection());
TypeFactoryRegister.Add<MyServiceWithMatchedIface>(c => new MyServiceWithMatchedIface());
TypeFactoryRegister.Add<MyServiceWithUniqueIFace>(c => new MyServiceWithUniqueIFace());
TypeFactoryRegister.Add<PrivateInner>(c => new PrivateInner());
TypeFactoryRegister.Add<SuggestedProxyMembershipProvider>(c => new SuggestedProxyMembershipProvider(c.Get<IMembershipProvider>()));
TypeFactoryRegister.Add<DataLazyConstructorInjection>(c => new DataLazyConstructorInjection(new Lazy<IMyService>(c.Get<IMyService>)));
TypeFactoryRegister.Add<DataFuncConstructorInjection>(c => new DataFuncConstructorInjection(c.Get<IMyService>));
TypeFactoryRegister.Add<DataLazyPropertyInjection>(c => new DataLazyPropertyInjection());
TypeFactoryRegister.Add<DataFuncPropertyInjection>(c => new DataFuncPropertyInjection());
TypeFactoryRegister.Add<MyInmemStorage>(c => new MyInmemStorage());
TypeFactoryRegister.Add<MyPipeStorage>(c => new MyPipeStorage());
TypeFactoryRegister.Add<MyFileStorage>(c => new MyFileStorage());
TypeFactoryRegister.Add<MyNodeHost>(c => new MyNodeHost());
TypeFactoryRegister.Add<MyObjectMan>(c => new MyObjectMan(c.Get<IMyStorage, MyPipeStorage>()));
TypeFactoryRegister.Add<MyPerformer>(c => new MyPerformer());
TypeFactoryRegister.Add<MyService>(c => new MyService());
TypeFactoryRegister.Add<MyService6WithoutInterface>(c => new MyService6WithoutInterface(c.Get<MyService7Dependency>()));
TypeFactoryRegister.Add<MyService7Dependency>(c => new MyService7Dependency());
#warning No constructor for type 'MyBadServiceDep'
/*
No constructor for type 'MyBadServiceDep'
Inner: No constructor for type 'MyBadServiceDep'
*/
TypeFactoryRegister.Add<MyBadService>(c => new MyBadService(c.Get<MyBadServiceDep>()));
TypeFactoryRegister.Add<MyService2>(c => new MyService2(c.Get<IMyService>()));
TypeFactoryRegister.Add<MyService3>(c => new MyService3(c.Get<IMyPerformer>()));
TypeFactoryRegister.Add<MyServiceWithOptionalArguments>(c => new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.TryGet<IMyWorker>()));
TypeFactoryRegister.Add<MyServiceWithString>(c => new MyServiceWithString(default(string)));
TypeFactoryRegister.Add<MyServiceWithStruct>(c => new MyServiceWithStruct(default(Guid)));
TypeFactoryRegister.Add<MyServiceWithOptionalStruct>(c => new MyServiceWithOptionalStruct());
TypeFactoryRegister.Add<MyServiceWithOptionalString>(c => new MyServiceWithOptionalString());
TypeFactoryRegister.Add<MyServiceWithStructPro>(c => new MyServiceWithStructPro());
TypeFactoryRegister.Add<MyServiceWithSeveralCtors>(c => new MyServiceWithSeveralCtors(c.Get<IMyService>(), c.Get<IMyService>()));
TypeFactoryRegister.Add<MySubGroup>(c => new MySubGroup(c.Get<Plasma.IPlasmaProvider>()));
TypeFactoryRegister.Add<MyWorker>(c => new MyWorker());
TypeFactoryRegister.Add<HibernateCrudDao<IMyService>>(c => new HibernateCrudDao<IMyService>());
TypeFactoryRegister.Add<HibernateCrudDao<IMyService2>>(c => new HibernateCrudDao<IMyService2>());
TypeFactoryRegister.Add<HibernateCrudDao<IMyService3>>(c => new HibernateCrudDao<IMyService3>());
TypeFactoryRegister.Add<Plasma.PlasmaContainer>(c => new Plasma.PlasmaContainer());

// Iface impl
FaceImplRegister.Register<ISession, Session>();
FaceImplRegister.Register<IMyService4, MyService4>();
FaceImplRegister.Register<IMyServiceComplex, MyService5>();
FaceImplRegister.Register<IMyServiceWithMatchedIface, MyServiceWithMatchedIface>();
FaceImplRegister.Register<IMyUniqueIFace, MyServiceWithUniqueIFace>();
#warning Cannot register service for type 'IPrivateIFace'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'IPrivateIFace'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.IPrivateIFace' not registered
*/
#warning Cannot register service for type 'ISimpleDataForStubbing'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'ISimpleDataForStubbing'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.Proxy.ISimpleDataForStubbing' not registered
*/
#warning Cannot register service for type 'IComplexStubbing'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'IComplexStubbing'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.Proxy.IComplexStubbing' not registered
*/
#warning Cannot register service for type 'IComplexStubbingDerived'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'IComplexStubbingDerived'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.Proxy.IComplexStubbingDerived' not registered
*/
#warning Cannot register service for type 'IComplexStubbingDerived2'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'IComplexStubbingDerived2'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.Proxy.IComplexStubbingDerived2' not registered
*/
#warning Cannot register service for type 'IComplexStubbingDerived3'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'IComplexStubbingDerived3'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.Proxy.IComplexStubbingDerived3' not registered
*/
#warning Cannot register service for type 'IMembershipProvider'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'IMembershipProvider'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'PlasmaTests.Sample.Proxy.IMembershipProvider' not registered
*/
FaceImplRegister.Register<IMyPerformer, MyPerformer>();
FaceImplRegister.Register<IMyService, MyService>();
FaceImplRegister.Register<IMyService2, MyService2>();
FaceImplRegister.Register<IMyService3, MyService3>();
FaceImplRegister.Register<IMyStorage, MyInmemStorage>();
FaceImplRegister.Register<IMyWorker, MyWorker>();
FaceImplRegister.Register<IMyServiceWithOptionalArguments, MyServiceWithOptionalArguments>();
FaceImplRegister.Register<Plasma.IPlasmaProvider, Plasma.PlasmaContainer>();

// Plumbers (Property injectors)
TypeAutoPlumberRegister.RegisterNone(typeof(Class1));
TypeAutoPlumberRegister.RegisterNone(typeof(SessionFactory));
TypeAutoPlumberRegister.RegisterNone(typeof(Session));
TypeAutoPlumberRegister.Register<GenericPerformer>((c, x)=>{
	x.Sdao2 = c.Get<HibernateCrudDao<IMyService2>>();
	x.Sdao3 = c.Get<HibernateCrudDao<IMyService3>>();
});
TypeAutoPlumberRegister.RegisterNone(typeof(MyGenericMethod));
TypeAutoPlumberRegister.Register<MyService4>((c, x)=>{
	x.Performer = c.Get<IMyPerformer>();
});
TypeAutoPlumberRegister.RegisterNone(typeof(MyService5));
TypeAutoPlumberRegister.Register<MyServiceWithAutomaticSetterOnlyInjection>((c, x)=>{
	x.Service = c.Get<IMyService>();
});
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithMatchedIface));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithUniqueIFace));
TypeAutoPlumberRegister.RegisterNone(typeof(PrivateInner));
TypeAutoPlumberRegister.RegisterNone(typeof(SuggestedProxyMembershipProvider));
TypeAutoPlumberRegister.RegisterNone(typeof(DataLazyConstructorInjection));
TypeAutoPlumberRegister.RegisterNone(typeof(DataFuncConstructorInjection));
TypeAutoPlumberRegister.Register<DataLazyPropertyInjection>((c, x)=>{
	x.LazyService = new Lazy<IMyService>(c.Get<IMyService>);
});
TypeAutoPlumberRegister.Register<DataFuncPropertyInjection>((c, x)=>{
	x.LazyService = c.Get<IMyService>;
});
TypeAutoPlumberRegister.RegisterNone(typeof(MyInmemStorage));
TypeAutoPlumberRegister.RegisterNone(typeof(MyPipeStorage));
TypeAutoPlumberRegister.RegisterNone(typeof(MyFileStorage));
TypeAutoPlumberRegister.Register<MyNodeHost>((c, x)=>{
	x.Storage = c.Get<IMyStorage, MyFileStorage>();
});
TypeAutoPlumberRegister.RegisterNone(typeof(MyObjectMan));
TypeAutoPlumberRegister.RegisterNone(typeof(MyPerformer));
TypeAutoPlumberRegister.Register<MyService>((c, x)=>{
	x.Worker = c.Get<IMyWorker>();
});
TypeAutoPlumberRegister.RegisterNone(typeof(MyService6WithoutInterface));
TypeAutoPlumberRegister.RegisterNone(typeof(MyService7Dependency));
TypeAutoPlumberRegister.RegisterNone(typeof(MyBadServiceDep));
TypeAutoPlumberRegister.RegisterNone(typeof(MyBadService));
TypeAutoPlumberRegister.RegisterNone(typeof(MyService2));
TypeAutoPlumberRegister.RegisterNone(typeof(MyService3));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithOptionalArguments));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithString));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithStruct));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithOptionalStruct));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithOptionalString));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithStructPro));
TypeAutoPlumberRegister.RegisterNone(typeof(MyServiceWithSeveralCtors));
TypeAutoPlumberRegister.RegisterNone(typeof(MySubGroup));
TypeAutoPlumberRegister.RegisterNone(typeof(MyWorker));
TypeAutoPlumberRegister.Register<HibernateCrudDao<IMyService>>((c, x)=>{
	x.Session = c.Get<ISession>();
});
TypeAutoPlumberRegister.Register<HibernateCrudDao<IMyService2>>((c, x)=>{
	x.Session = c.Get<ISession>();
});
TypeAutoPlumberRegister.Register<HibernateCrudDao<IMyService3>>((c, x)=>{
	x.Session = c.Get<ISession>();
});
TypeAutoPlumberRegister.RegisterNone(typeof(Plasma.PlasmaContainer));
Null.Register<ISession>(NullSession.Instance);
Null.Register<IMyService4>(NullMyService4.Instance);
Null.Register<IMyServiceComplex>(NullMyServiceComplex.Instance);
Null.Register<IMyServiceWithMatchedIface>(NullMyServiceWithMatchedIface.Instance);
Null.Register<IMyUniqueIFace>(NullMyUniqueIFace.Instance);
Null.Register<IPrivateIFace>(NullPrivateIFace.Instance);
Null.Register<ISimpleDataForStubbing>(NullSimpleDataForStubbing.Instance);
Null.Register<IComplexStubbing>(NullComplexStubbing.Instance);
Null.Register<IComplexStubbingDerived>(NullComplexStubbingDerived.Instance);
Null.Register<IComplexStubbingDerived2>(NullComplexStubbingDerived2.Instance);
Null.Register<IComplexStubbingDerived3>(NullComplexStubbingDerived3.Instance);
Null.Register<IMembershipProvider>(NullMembershipProvider.Instance);
Null.Register<IMyPerformer>(NullMyPerformer.Instance);
Null.Register<IMyService>(NullMyService.Instance);
Null.Register<IMyService2>(NullMyService2.Instance);
Null.Register<IMyService3>(NullMyService3.Instance);
Null.Register<IMyStorage>(NullMyStorage.Instance);
Null.Register<IMyWorker>(NullMyWorker.Instance);
Null.Register<IMyServiceWithOptionalArguments>(NullMyServiceWithOptionalArguments.Instance);
Null.RegisterGeneric(typeof(IEnumerable<>), t =>
	typeof (NullEnumerable<>).MakeGenericType(t).GetField("Instance").GetValue(null));
Null.RegisterGeneric(typeof(IList<>), t =>
	typeof (NullList<>).MakeGenericType(t).GetField("Instance").GetValue(null));
Null.RegisterGeneric(typeof(IEnumerator<>), t =>
	typeof (NullEnumerator<>).MakeGenericType(t).GetField("Instance").GetValue(null));
Null.Register<System.Collections.IEnumerator>(NullEnumerator.Instance);

	}
}




}
