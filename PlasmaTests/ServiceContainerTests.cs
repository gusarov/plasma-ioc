using System.Linq;
using System.Collections.Generic;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;
using MetaCreator;

using PlasmaTests.Sample;

namespace PlasmaTests
{
	[TestClass]
	public class ServiceContainerTests : BaseContainer
	{

		[TestMethod]
		public void Should_allow_store_instance_of_service()
		{
			// Setup
			var ms = new MyService();
			_sut.Add<IMyService>(ms);

			// Execute
			// Verify
			Assert.AreSame(ms, _sut.Get<IMyService>());
			Assert.AreSame(ms, _sut.Get<IMyService>());
		}

		[Ignore] // disabled
		[TestMethod]
		[ExpectedException(typeof(PlasmaException))]
		public void Should_throw_on_unknown_service()
		{
			_sut.Get<MyService>();
		}

		[TestMethod]
		public void Should_return_null_on_try_unknown_service()
		{
			// Execute
			Assert.AreEqual(null, _sut.TryGet(typeof(MyService)));
			Assert.AreEqual(null, _sut.TryGet<MyService>());
		}

		[TestMethod]
		public void Should_allow_check_existence()
		{
			Assert.IsNull(_sut.TryGet(typeof(IMyService)));
			Assert.IsNull(_sut.TryGet<IMyService>());
			_sut.Get<IMyService>(); // autofactory
			Assert.IsNotNull(_sut.TryGet(typeof(IMyService)));
			Assert.IsNotNull(_sut.TryGet<IMyService>());
		}

		[TestMethod]
		[ExpectedException(typeof(PlasmaException))]
		public void Should_not_allow_access_instance_by_class()
		{
			Assert.Inconclusive("Produce warning instead! Or exception with smart detailed explanation");
			// Setup
			var ms = new MyService();
			_sut.Add<IMyService>(ms); // register this service instance by type

			// Execute
			_sut.Get<MyService>();
		}

		[TestMethod]
		public void Should_use_most_fresh_instance()
		{
			// Setup
			var ser1 = new MyService();
			var ser2 = new MyService();
			_sut.Add<IMyService>(ser1);
			_sut.Add<IMyService>(ser2);

			// Execute
			// Verify
			Assert.AreSame(ser2, _sut.Get<IMyService>());
			Assert.AreSame(ser2, _sut.Get<IMyService>());
		}

		[TestMethod]
		public void Should_register_interface_automatically()
		{
			// Setup
			var ms = new MyService();
			_sut.Add(ms); // register this service instance and i'ts single interface

			Assert.AreEqual(ms, _sut.Get<IMyService>());
			Assert.AreEqual(ms, _sut.Get<MyService>());
		}

		[TestMethod]
		public void Should_register_interface_automatically_only_for_one_iface()
		{
			// Setup
			var ms = new MyService2(null);
			_sut.Add(ms);
			try
			{
				_sut.Get<IMyService2>();
				Assert.Fail();
			}
			catch (PlasmaException) {}
		}

		[TestMethod]
		public void Should_create_factory_automatically()
		{
			// Setup
			MyService.Instantiated = 0;
			_sut.Add<IMyService>(); // register lazy service factoryMethod by interface type
			Assert.AreEqual(0, MyService.Instantiated);
			// Execute
			var ms = _sut.Get<IMyService>();
			Assert.IsInstanceOfType(ms, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
		}

		[TestMethod]
		public void Should_create_factory_automatically_by_class()
		{
			// Setup
			_sut.Add<MyService>();
			// Execute
			var ms1 = _sut.Get<IMyService>();
			var ms2 = _sut.Get<IMyService>();
			Assert.IsInstanceOfType(ms1, typeof(MyService));
			Assert.IsInstanceOfType(ms2, typeof(MyService));
			Assert.AreSame(ms1, ms2);
		}

		[TestMethod]
		public void Should_create_factory_automatically_by_class_and_iface()
		{
			// Setup
			_sut.Add<IMyService2, MyService2>();

			// Execute
			var ms1 = _sut.Get<IMyService2>();
			Assert.IsInstanceOfType(ms1, typeof(MyService2));
			Assert.IsInstanceOfType(ms1.SubService, typeof(MyService));
		}

		[TestMethod]
		public void Should_allow_get_service_by_smart_interface_with_attribute()
		{
			// Setup
			// Execute
			// Verify
			var ms1 = _sut.Get<IMyService>();
			var ms2 = _sut.Get<IMyService>();
			Assert.IsInstanceOfType(ms1, typeof(MyService));
			Assert.IsInstanceOfType(ms2, typeof(MyService));
			Assert.AreSame(ms1, ms2);
		}

		interface IMyDisposable
		{
			
		}

		class MyDisposable : IMyDisposable, IDisposable
		{
			public bool IsDisposed { get; private set; }
			void IDisposable.Dispose()
			{
				IsDisposed = true;
			}
		}

		[TestMethod]
		public void Should_dispose_all_services_on_disposal()
		{
			// Setup
			var md = new MyDisposable();
			_sut.Add<IMyDisposable>(md);
			// Execute
			_sut.Dispose();
			// Verify
			Assert.AreEqual(true, md.IsDisposed);
		}

		[TestMethod]
		public void Should_get_or_create_by_provided_factory()
		{
			// Setup
			var now = _sut.TryGet<IMyService2>();
			Assert.IsNull(now);

			var place = new MyService2(null);

			int cnt = 0;

			now = _sut.Get<IMyService2>(() => { cnt++; return place; });

			Assert.AreEqual(place, now);
			Assert.AreEqual(1, cnt);
			now = _sut.Get<IMyService2>(() => { cnt++; return place; });
			Assert.AreEqual(1, cnt);
		}

		[TestMethod]
		public void Should_provide_default_impl_suggestion_on_ctor_injection()
		{
			// Setup
			// automatic
			_sut.Add<MyNodeHost>(); 

			// Execute
			var nodeHost = _sut.Get<MyNodeHost>();

			// Verify
			Assert.IsInstanceOfType(nodeHost.Storage, typeof(MyFileStorage));
		}

		[TestMethod]
		public void Should_provide_default_impl_suggestion_on_ctor_injection_only_if_there_are_no_impl()
		{
			// Setup
			_sut.Add<MyNodeHost>();
			_sut.Add<MyPipeStorage>();

			// Execute
			var nodeHost = _sut.Get<MyNodeHost>();

			// Verify
			Assert.IsInstanceOfType(nodeHost.Storage, typeof(MyPipeStorage));
		}

		[TestMethod]
		public void Should_provide_default_impl_suggestion_on_property_injection()
		{
			// Setup
			// automatic
			_sut.Add<MyObjectMan>();

			// Execute
			var nodeHost = _sut.Get<MyObjectMan>();

			// Verify
			Assert.IsInstanceOfType(nodeHost.Storage, typeof(MyPipeStorage));
		}

		[TestMethod]
		public void Should_provide_default_impl_suggestion_on_get()
		{
			// Setup
			// automatic

			// Execute
			var storage = _sut.Get<IMyStorage>();

			// Verify
			Assert.IsInstanceOfType(storage, typeof(MyInmemStorage));
		}

		[TestMethod]
		public void Should_provide_suggested_impl_on_get()
		{
			// Setup
			// automatic

			// Execute
			var storage = _sut.Get<IMyStorage, MyPipeStorage>();

			// Verify
			Assert.IsInstanceOfType(storage, typeof(MyPipeStorage));
		}

		[TestMethod]
		public void Should_not_provide_suggested_impl_on_get_if_already_instantiated()
		{
			// Setup
			// automatic

			// Execute
			var storage = _sut.Get<IMyStorage>();
			storage = _sut.Get<IMyStorage, MyPipeStorage>();

			// Verify
			Assert.IsInstanceOfType(storage, typeof(MyInmemStorage));
		}

		[TestMethod]
		public void Should_not_provide_suggested_impl_on_get_if_already_requested()
		{
			// Setup
			// automatic

			// Execute
			var storage = _sut.GetLazy(typeof(IMyStorage));
			var storage2 = _sut.Get<IMyStorage, MyPipeStorage>();

			// Verify
			Assert.IsInstanceOfType(storage2, typeof(MyInmemStorage));
		}

		struct MyStruct : IComparable
		{
			#region Implementation of IComparable

			public int CompareTo(object obj)
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		[TestMethod]
		[ExpectedException(typeof(PlasmaException))]
		public void Should_not_allow_register_value_types()
		{
			_sut.Add<IComparable, MyStruct>();
		}

		[TestMethod]
		[ExpectedException(typeof(PlasmaException))]
		public void Should_not_allow_register_value_types2()
		{
			_sut.Add<IComparable>(new MyStruct());
		}

		[TestMethod]
		[ExpectedException(typeof(PlasmaException))]
		public void Should_not_allow_register_value_types3()
		{
			_sut.Add(new MyStruct());
		}

		[TestMethod]
		public void Should_allow_construct_class_with_struct_parameters()
		{
			// Setup
			_sut.Add<MyServiceWithStruct>();

			// Execute
			// Verify
			_sut.Get<MyServiceWithStruct>();
		}

		[TestMethod]
		public void Should_allow_construct_class_with_string_parameters()
		{
			// Setup
			_sut.Add<MyServiceWithString>();

			// Execute
			// Verify
			_sut.Get<MyServiceWithString>();
		}

		[TestMethod]
		public void Should_allow_construct_class_with_string_and_struct_properties()
		{
			// Setup
			_sut.Add<MyServiceWithStructPro>();
			// Execute
			// Verify
			var test = _sut.Get<MyServiceWithStructPro>();
			Assert.AreEqual("asd", test.Test);
		}

#if NET4

		[TestMethod]
		public void Should_ignore_absent_optional_services()
		{
			// Setup
			// Execute
			var test = _sut.Get<IMyServiceWithOptionalArguments>();
			// Verify

			Assert.IsNotNull(test.Storage); // non optional argument
			Assert.IsNull(test.Worker); // optional argument
		}

		[TestMethod]
		public void Should_not_ignore_existing_optional_services()
		{
			// Setup
			_sut.Get<IMyWorker>(); // autoinit
			// Execute
			var test = _sut.Get<IMyServiceWithOptionalArguments>();
			// Verify

			Assert.IsNotNull(test.Storage); // non optional argument
			Assert.IsNotNull(test.Worker); // optional argument
		}

		[TestMethod]
		public void Should_use_default_value_of_optional_parameter_for_structs()
		{
			// Setup
			// Execute
			_sut.Add<MyServiceWithOptionalStruct>();

			var test = _sut.Get<MyServiceWithOptionalStruct>();
			// Verify
			Assert.AreEqual(true, test.Val);
		}

		[TestMethod]
		public void Should_use_default_value_of_optional_parameter_for_strings()
		{
			// Setup
			// Execute
			_sut.Add<MyServiceWithOptionalString>();
			var test = _sut.Get<MyServiceWithOptionalString>();
			// Verify
			Assert.AreEqual("test", test.Val);
		}

#endif

		[TestMethod]
		public void Should_create_service_by_apropriate_ctor()
		{
			_sut.Add<MyServiceWithSeveralCtors>();
			var test = _sut.Get<MyServiceWithSeveralCtors>();
			Assert.IsNotNull(test);
			Assert.IsNotNull(test.Service1);
			Assert.IsNotNull(test.Service2);
			Assert.IsNull(test.Service3);
		}

		[TestMethod]
		public void Should_instantiate_class_without_ctor()
		{
			var test = _sut.Get<IMyWorker>();
			Assert.IsNotNull(test);
		}

		[TestMethod]
		public void Should_generate_type_name_correctly_for_kvp()
		{
			Assert.AreEqual("KeyValuePair<int,int>[]", typeof(KeyValuePair<int, int>[]).CSharpTypeIdentifier("MySpace", "System.Collections.Generic"));
		}

		[TestMethod]
		public void Should_instantiate_service_without_interface()
		{
			var svc = _sut.Get<MyService6WithoutInterface>();
			Assert.IsNotNull(svc);
			Assert.IsNotNull(svc.Service);
		}
	}
}
