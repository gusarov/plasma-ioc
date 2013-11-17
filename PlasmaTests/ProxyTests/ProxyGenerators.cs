using Microsoft.VisualStudio.TestTools.UnitTesting;

using PlasmaTests.Precompiler;
using PlasmaTests.Sample.Proxy;

namespace PlasmaTests.Proxy
{
	[TestClass]
	public class ProxyGenerators
	{
		[TestMethod]
		public void Should_generate_simple_universal_proxy()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void Should_generate_null_objects()
		{
			Assert.Inconclusive();
		}

		[TestMethod]
		public void Should_generate_stubs()
		{
			Assert.Inconclusive();
		}
	}

	/*
	public class SecurityProxy : ProxyMembershipProvider
	{
		public SecurityProxy(IMembershipProvider originalObject) : base(originalObject)
		{
		}

		public override void DeleteUser(string login)
		{
			// verify permissions
			base.DeleteUser(login);
		}
	}
	 * */
}
