using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plasma;
using PlasmaTests.Precompiler;
using PlasmaTests.Sample;
using PlasmaTests.Sample.Proxy;

namespace PlasmaTests.Proxy
{
	[TestClass]
	public class TestNullObject
	{
		static TestNullObject()
		{
			_AssemblyInit.Init(null);
		}

		[TestMethod]
		public void Should_verify_null_object_behavior_of_iface()
		{
			var sut = Null.Object<IMembershipProvider>();
			sut.AddUser(null, null);
			var enumerable = sut.ListUsers();
			Assert.IsNotNull(enumerable);
			Assert.IsTrue(!enumerable.Any());
			Assert.IsTrue(!sut.ListUsers2().Any());
			Assert.IsFalse(sut.ValidateUser("asd", "asd"));
		}

		[TestMethod]
		public void Should_use_out()
		{
			var sut = Null.Object<IMyServiceComplex>();
			int q;
			var r = sut.MyMethod(4, out q);
			Assert.AreEqual(0, q);
			Assert.AreEqual(false, r);
		}

		[TestMethod]
		[ExpectedException(typeof(PlasmaException))]
		public void Should_not_allow_register_2_times()
		{
			Null.Register<IMyServiceComplex>(new MyService5());
		}

		[TestMethod]
		public void Should_return_generic_nullable()
		{
			var test1 = Null.Object<IEnumerable<int>>();
			var test2 = Null.Object<IEnumerable<string>>();

			Assert.IsNotNull(test1);
			Assert.IsNotNull(test2);
			Assert.AreEqual(0, test1.Count());
			Assert.AreEqual(0, test2.Count());
		}
	}
}
