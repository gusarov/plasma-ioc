using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

using TurboFacTests.Sample;

namespace TurboFacTests
{
	[TestClass]
	public class ContainerHierarchy
	{
		readonly ITurboContainer _sut = new TurboContainer();

		[TestMethod]
		public void Should_allow_to_have_several_subcontainers_with_common_and_different_instances()
		{
			// Setup
			var myPerformer = new MyPerformer();
			_sut.Add(myPerformer);

			Assert.AreEqual(myPerformer, _sut.Get<IMyPerformer>());
			Assert.AreEqual(myPerformer, _sut.Get<MyPerformer>());

			var group1 = new MySubGroup(new TurboContainer(_sut));
			var group2 = new MySubGroup(new TurboContainer(_sut));


			Assert.AreSame(myPerformer, group1.Performer);
			Assert.AreSame(myPerformer, group2.Performer);

			Assert.AreNotSame(group1.Worker, group2.Worker);
		}
	}
}
