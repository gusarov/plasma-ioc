using System;
using System.Collections.Generic;
using System.Linq;

using Plasma;

using PlasmaTests.Precompiler;

namespace TurboFacTests
{
	partial class _AssemblyInit
	{
		static partial void Init(IPlasmaContainer c)
		{
			PerformRegistration.Perform(c);
		}
	}
}
