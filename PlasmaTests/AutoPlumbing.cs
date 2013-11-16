using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plasma;
using PlasmaTests.Sample;

namespace PlasmaTests
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
			_sut.Add(new MyPerformer());
			var test = _sut.Get<IMyService3>();
			Assert.IsInstanceOfType(test, typeof(MyService3));
			Assert.AreEqual(true, test.MyMethod());
		}

		[TestMethod]
		public void Should_glue_two_ifaces_through_property()
		{
			_sut.Add(new MyPerformer());
			var test = _sut.Get<IMyService4>();
			Assert.IsInstanceOfType(test, typeof(MyService4));
			Assert.AreEqual(true, test.MyMethod());
		}

		[TestMethod]
		public void Should_glue_setter_only_injection_without_attribute()
		{
			var svcInj = _sut.Get<IMyService>();
			var svc = _sut.Get<MyServiceWithAutomaticSetterOnlyInjection>();
			Assert.AreEqual(svcInj, svc.__service);
		}

		[TestMethod]
		public void MyTestMethod()
		{
			var ps = PlasmaContainer.GetPlumbingProperties(typeof(MyServiceWithAutomaticSetterOnlyInjection));

			Assert.AreEqual(1, ps.Count());

		
		}

	}
}