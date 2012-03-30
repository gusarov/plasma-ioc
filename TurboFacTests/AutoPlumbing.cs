using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

using TurboFacTests.Sample;

namespace TurboFacTests
{
	[TestClass]
	public class AutoPlumbing
	{
		readonly ITurboContainer _sut = new TurboContainer();

		[TestMethod]
		public void Should_glue_two_ifaces_on_the_fly()
		{
			var test = _sut.Get<IMyService>();
			Assert.IsInstanceOfType(test, typeof (MyService));
			Assert.AreEqual(777, test.MyMethod());
		}

		[TestMethod]
		public void Should_glue_two_ifaces_through_ctor()
		{
			_sut.Add(new MyPerformer());
			var test = _sut.Get<IMyService3>();
			Assert.IsInstanceOfType(test, typeof(MyService3));
			Assert.AreEqual(true, test.MyMethod());
		}
	}
}
