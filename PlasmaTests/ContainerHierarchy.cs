using System.Linq;
using System.Collections.Generic;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;

using PlasmaTests.Sample;

namespace PlasmaTests
{
	[TestClass]
	public class ContainerHierarchy : BaseContainer
	{

		[TestMethod]
		public void Should_allow_to_have_several_subcontainers_with_common_and_different_instances()
		{
			// Setup
			var myPerformer = new MyPerformer();
			_sut.Add(myPerformer);

			Assert.AreEqual(myPerformer, _sut.Get<IMyPerformer>());
			Assert.AreEqual(myPerformer, _sut.Get<MyPerformer>());

			var group1 = new MySubGroup(new PlasmaContainer(_sut));
			var group2 = new MySubGroup(new PlasmaContainer(_sut));


			Assert.AreSame(myPerformer, group1.Performer);
			Assert.AreSame(myPerformer, group2.Performer);

			Assert.AreNotSame(group1.Worker, group2.Worker);
		}
	}
}
