using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

using TurboFacTests.Sample;

namespace TurboFacTests
{
	[TestClass]
	public class AutoPlumbing : BaseContainer
	{

		[TestMethod]
		public void Should_glue_two_ifaces_on_the_fly()
		{
			var test = _sut.Get<IMyService>();
			Assert.IsInstanceOfType(test, typeof(MyService));
			Assert.AreEqual(777, test.MyMethod());
		}

		[TestMethod]
		public void Should_glue_two_ifaces_through_ctor()
		{
#if !PRE
			_sut.Add(new MyPerformer());
#endif
			var test = _sut.Get<IMyService3>();
			Assert.IsInstanceOfType(test, typeof(MyService3));
			Assert.AreEqual(true, test.MyMethod());
		}

		[TestMethod]
		public void Should_glue_two_ifaces_through_property()
		{
#if !PRE
			_sut.Add(new MyPerformer());
#endif
			var test = _sut.Get<IMyService4>();
			Assert.IsInstanceOfType(test, typeof(MyService4));
			Assert.AreEqual(true, test.MyMethod());
		}
	}
}
