using System.Linq;
using System.Collections.Generic;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;

using PlasmaTests.Sample;

namespace PlasmaTests
{
	[TestClass]
	public class LazyConverter
	{
		[TestMethod]
		public void Should_convert_lazy_object_to_typed()
		{
			MyService.Instantiated = 0;

			var sut = new TypedLazyWrapper(typeof(IMyService), new Lazy<object>(() => new MyService()));

			var result = sut.Lazy;

			Assert.IsInstanceOfType(result, typeof(Lazy<IMyService>));

			var typed = (Lazy<IMyService>)result;

			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(typed.Value, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
			Assert.IsInstanceOfType(typed.Value, typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);

		}
		[TestMethod]
		public void Should_convert_lazy_object_to_typed_func()
		{
			MyService.Instantiated = 0;

			var sut = new TypedFuncWrapper(typeof(IMyService), new Lazy<object>(() => new MyService()));

			var result = sut.Func;

			Assert.IsInstanceOfType(result, typeof(Func<IMyService>));

			var typed = (Func<IMyService>)result;

			Assert.AreEqual(0, MyService.Instantiated);
			Assert.IsInstanceOfType(typed(), typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);
			Assert.IsInstanceOfType(typed(), typeof(MyService));
			Assert.AreEqual(1, MyService.Instantiated);

		}
	}
}
