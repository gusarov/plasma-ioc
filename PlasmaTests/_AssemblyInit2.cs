using System;
using System.Collections.Generic;
using System.Linq;

using Plasma;

using PlasmaTests.Precompiler;

namespace PlasmaTests
{
	partial class _AssemblyInit
	{
		static partial void Init(IPlasmaContainer c)
		{
			PerformRegistration.Perform(c);
		}
	}
}
