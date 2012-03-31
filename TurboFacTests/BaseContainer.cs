using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

namespace TurboFacTests
{
	[TestClass]
	public abstract class BaseContainer
	{
		protected ITurboContainer _sut;

		[TestInitialize]
		public void BaseContainerInit()
		{
			// empty container
			_sut = new TurboContainer();
			_AssemblyInit.InitContainer(_sut);
		}
	}
}
