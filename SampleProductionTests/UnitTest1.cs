using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MySpring;

using SampleProduction;

using TurboFac;

namespace SampleProductionTests
{
	[TestClass]
	public class UnitTest1
	{
		readonly ISpringContainer _spring = SpringContainer.Root;

		public UnitTest1()
		{
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
