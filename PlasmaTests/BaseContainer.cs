using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;

namespace PlasmaTests
{
	[TestClass]
	public abstract class BaseContainer
	{
		protected IPlasmaContainer _sut;

		[TestInitialize]
		public void BaseContainerInit()
		{
			// empty container
			_sut = new PlasmaContainer();
			_AssemblyInit.InitContainer(_sut);
		}
	}
}
