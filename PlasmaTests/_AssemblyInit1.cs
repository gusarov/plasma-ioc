using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plasma;

namespace TurboFacTests
{
	[TestClass]
	public static partial class _AssemblyInit
	{
		public static ReflectionPermission ReflectionPermission;

		[AssemblyInitialize]
		public static void Init(TestContext ctx)
		{
			PlasmaContainer.DefaultReflectionPermission =
			ReflectionPermission = typeof(_AssemblyInit).Assembly.FullName.Contains("Precompiled")
				? (Debugger.IsAttached
					? ReflectionPermission.Throw
					: ReflectionPermission.Throw)
				: ReflectionPermission.Allow;
			Init(PlasmaContainer.Root);
		}

		static partial void Init(IPlasmaContainer c);

		public static void InitContainer(IPlasmaContainer c)
		{
			Init(c);
		}
	}
}
