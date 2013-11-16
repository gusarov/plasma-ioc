using System.Linq;
using System.Collections.Generic;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;

namespace PlasmaTests
{
	[TestClass]
	public class AppConfigUsage
	{
		#region Classes
		public interface IOne
		{

		}

		[DefaultImpl(typeof(Two))]
		public interface ITwo
		{
			IOne OneA { get; set; }
			IOne OneB { get; set; }
		}

		public class One1 : IOne
		{

		}

		public class One2 : IOne
		{

		}

		public class Two : ITwo
		{
			public IOne OneA { get; set; }
			public IOne OneB { get; set; }
		} 
		#endregion

		[TestMethod]
		public void Should_pre_initialize_provider_using_app_config()
		{
			Assert.Inconclusive();
			// Setup
			var sut = new PlasmaContainer();

			// Execute
			var two = sut.Get<ITwo>();

			// Verify
			Assert.IsInstanceOfType(two.OneA, typeof(One1));
			Assert.IsInstanceOfType(two.OneB, typeof(One2));
		}
	}
}
