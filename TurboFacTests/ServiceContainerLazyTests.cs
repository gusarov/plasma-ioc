using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

namespace TurboFacTests
{
	[TestClass]
	public class ServiceContainerLazyTests
	{
		#region Data

		interface IMyService { }

		[DefaultImpl(typeof(MySelfService))]
		interface IMySelfService { }

		class MySelfService : IMySelfService
		{
			public MySelfService()
			{
				Instantiated++;
			}
		}

		class MyService : IMyService
		{
			public MyService()
			{
				Instantiated++;
			}
		}

		class MyService2 : IMyService { }

		#endregion

		ITurboContainer _sut;

		[TestInitialize]
		public void Init()
		{
			// Setup
			_sut = new TurboContainer();
			Instantiated = 0;
		}

		public static int Instantiated { get; set; }

		void Verify()
		{
			Verify<IMyService, MyService>();
		}

		void Verify<TI, TC>()
		{
			Assert.AreEqual(0, Instantiated);

			// Execute
			var test = _sut.Get<TI>();
			Assert.IsInstanceOfType(test, typeof(TC));

			Assert.AreEqual(1, Instantiated);

			Assert.AreSame(test, _sut.Get<TI>());

			Assert.AreEqual(1, Instantiated);
		}

		[TestMethod]
		public void Should_allow_register_func_factory_by_iface_and_lambda()
		{
			_sut.Add<IMyService>(() => new MyService());
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_func_factory_by_lambda()
		{
			_sut.Add(() => new MyService());
			Verify();
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_func_factory_by_object()
		{
			_sut.Add((object)(Func<object>)(() => new MyService()));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_func_factory_by_object2()
		{
			_sut.Add((object)(Func<IMyService>)(() => new MyService()));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_func_factory_by_object3()
		{
			_sut.Add((object)(Func<MyService>)(() => new MyService()));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_func_factory_by_object_and_type()
		{
			_sut.Add(typeof(IMyService), (object)(Func<object>)(() => new MyService()));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_func_factory_by_object2_and_type()
		{
			_sut.Add(typeof(IMyService), (object)(Func<IMyService>)(() => new MyService()));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_func_factory_by_object3_and_type()
		{
			_sut.Add(typeof(IMyService), (object)(Func<MyService>)(() => new MyService()));
		}

		[TestMethod]
		public void Should_allow_register_funk_factory_by_type()
		{
			var factory = new Func<object>(() => new MyService());
			_sut.Add(typeof(IMyService), factory);
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_funk_factory_by_impltype()
		{
			var factory = new Func<object>(() => new MyService());
			_sut.Add(typeof(MyService), factory);
			Verify();
		}

		// =============================

		[TestMethod]
		public void Should_allow_register_lazy_factory1()
		{
			_sut.Add<IMyService>(new Lazy<IMyService>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory2()
		{
			_sut.Add(new Lazy<IMyService>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory3()
		{
			_sut.Add<IMyService, MyService>(new Lazy<MyService>());
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory4()
		{
			_sut.Add<IMyService, MyService>();
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory5()
		{
			_sut.Add(new Lazy<IMyService>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory6()
		{
			_sut.Add(typeof(IMyService), new Lazy<IMyService>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory7()
		{
			_sut.Add(typeof(IMyService), new Lazy<object>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory8()
		{
			_sut.Add<object>(typeof(IMyService), new Lazy<object>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		public void Should_allow_register_lazy_factory9()
		{
			_sut.Add<IMyService>(typeof(IMyService), new Lazy<IMyService>(() => new MyService()));
			Verify();
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_lazy_factory_by_object()
		{
			_sut.Add((object)new Lazy<object>((() => new MyService())));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_lazy_factory_by_object2()
		{
			_sut.Add((object)new Lazy<IMyService>(() => new MyService()));
		}
		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_lazy_factory_by_object3()
		{
			_sut.Add((object)new Lazy<MyService>(() => new MyService()));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_lazy_factory_by_object_and_type()
		{
			_sut.Add(typeof(IMyService), (object)new Lazy<object>((() => new MyService())));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_lazy_factory_by_object2_and_type()
		{
			_sut.Add(typeof(IMyService), (object)new Lazy<IMyService>((() => new MyService())));
		}

		[TestMethod]
		[ExpectedException(typeof(TurboFacException))]
		public void Should_not_allow_register_lazy_factory_by_object3_and_type()
		{
			_sut.Add(typeof(IMyService),(object)new Lazy<MyService>((() => new MyService())));
		}

		[TestMethod]
		public void Should_register_default_factory()
		{
			_sut.Add(typeof(IMySelfService));
			Verify<IMySelfService, MySelfService>();
		}

		[TestMethod]
		public void Should_register_default_factory2()
		{
			_sut.Add(typeof(IMyService), typeof(MyService));
			Verify();
		}

		[TestMethod]
		public void Should_register_default_factory3()
		{
			_sut.Add<IMySelfService>();
			Verify<IMySelfService, MySelfService>();
		}

		[TestMethod]
		public void Should_register_default_factory4()
		{
			_sut.Add<MyService>();
			Verify();
		}

		[TestMethod]
		public void Should_register_default_factory5()
		{
			_sut.Add<IMyService, MyService>();
			Verify();
		}

		[TestMethod]
		public void Should_register_default_factory6()
		{
			_sut.Add(typeof(MyService));
			Verify();
		}

	}
}