using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plasma;
using Plasma.Meta;
using Spring.Objects.Factory;

namespace PlasmaTests
{
	[TestClass]
	public class MiningTests
	{
		[TestMethod]
		public void MyTestMethod()
		{
			object ref2 = typeof(Spring.Objects.Factory.Config.ObjectFactoryCreatingFactoryObject);

			AssemblyAnalyzeCache.AnalyzeImpls();

			var impls = AssemblyAnalyzeCache.ImplsOf(typeof(IGenericObjectFactory));
			Assert.IsTrue(impls == null || impls.Count == 0);

			var mining = new StaticMining();
			Type impl = null;
			try
			{
				impl = mining.IfaceImpl(typeof(IGenericObjectFactory));
			}
			catch(PlasmaException)
			{
			}
			Assert.IsNull(impl);

			Console.WriteLine();
		}
	}
}
