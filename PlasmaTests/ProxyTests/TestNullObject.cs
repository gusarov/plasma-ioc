using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PlasmaTests.Sample.Proxy;

namespace PlasmaTests.Proxy
{
	[TestClass]
	public class TestNullObject
	{
		[TestMethod]
		public void Should_verify_null_object_behavior_of_iface()
		{
			var sut = Null.Object<IMembershipProvider>();
			sut.AddUser(null, null);
			var enumerable = sut.ListUsers();
			Assert.IsNotNull(enumerable);
			Assert.IsTrue(!enumerable.Any());
			Assert.IsFalse(sut.ValidateUser("asd", "asd"));
		}
	}
}
