using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

using TurboFacTests.Precompiler;

namespace TurboFacTests
{
	partial class _AssemblyInit
	{
		static partial void Init(ITurboContainer c)
		{
			PerformRegistration.Perform(c);
		}
	}
}
