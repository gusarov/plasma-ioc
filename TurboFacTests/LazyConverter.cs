using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

using TurboFacTests.Sample;

namespace TurboFacTests
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

		}
	}
}
