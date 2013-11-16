using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;

using SampleProduction;

namespace SampleProductionTests
{
	[TestClass]
	public class UnitTest1
	{
		readonly IPlasmaContainer _spring = PlasmaContainer.Root;

		public UnitTest1()
		{
			PlasmaContainer.DefaultReflectionPermission = ReflectionPermission.Throw;
			AutoStub.RegisterAll(_spring);
		}

		[TestMethod]
		public void TestMethod1()
		{
			var sut = _spring.Get<MovieLister>();
			var movie = sut.Find("m").Single();
			Assert.AreEqual("tmp", movie.Name);
		}
	}
}
