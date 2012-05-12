using System.Linq;
using System.Collections.Generic;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PlasmaTests.Sample;


namespace PlasmaTests
{
	[TestClass]
	public class ConstructorAndPropertyLazyInjection : BaseContainer
	{

		[TestInitialize]
		public void Init()
		{
			DataLazyConstructorInjection.Created = 0;
			DataLazyPropertyInjection.Created = 0;
			DataFuncConstructorInjection.Created = 0;
			DataFuncPropertyInjection.Created = 0;
			MyService.Instantiated = 0;
		}

		[TestMethod]
		public void Should_inject_all_deps_to_ctor()
		{
			// Setup
			_sut.Add<DataLazyConstructorInjection>();
			// Execute
			Assert.AreEqual(0, DataLazyConstructorInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			var sample = _sut.Get<DataLazyConstructorInjection>();
			Assert.AreEqual(0, MyService.Instantiated);
			// Verify
			Assert.AreEqual(1, DataLazyConstructorInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample, typeof(DataLazyConstructorInjection));
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.MyService, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.MyService, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
		}

		[TestMethod]
		public void Should_inject_all_deps_to_ctor_func()
		{
			// Setup
			_sut.Add<DataFuncConstructorInjection>();
			
			// Execute
			Assert.AreEqual(0, DataFuncConstructorInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			var sample = _sut.Get<DataFuncConstructorInjection>();
			Assert.AreEqual(0, MyService.Instantiated);
			// Verify
			Assert.AreEqual(1, DataFuncConstructorInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample, typeof(DataFuncConstructorInjection));
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.MyService, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.MyService, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
		}

		[TestMethod]
		public void Should_inject_all_deps_to_prop()
		{
			// Setup
			_sut.Add<DataLazyPropertyInjection>();

			// Execute
			Assert.AreEqual(0, DataLazyPropertyInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			var sample = _sut.Get<DataLazyPropertyInjection>();
			Assert.AreEqual(0, MyService.Instantiated);
			// Verify
			Assert.AreEqual(1, DataLazyPropertyInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample, typeof(DataLazyPropertyInjection));
			Assert.IsNotNull(sample.LazyService);
			Assert.IsFalse(sample.LazyService.IsValueCreated);
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.LazyService.Value, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.LazyService.Value, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
		}

		[TestMethod]
		public void Should_inject_all_deps_to_prop_func()
		{
			// Setup
			_sut.Add<DataFuncPropertyInjection>();

			// Execute
			Assert.AreEqual(0, DataFuncPropertyInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			var sample = _sut.Get<DataFuncPropertyInjection>();
			Assert.AreEqual(0, MyService.Instantiated);
			// Verify
			Assert.AreEqual(1, DataFuncPropertyInjection.Created);
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample, typeof(DataFuncPropertyInjection));
			Assert.IsNotNull(sample.LazyService);
			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.LazyService(), typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
			Assert.IsInstanceOfType(sample.LazyService(), typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
		}
	}
}
