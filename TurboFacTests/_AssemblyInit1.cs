using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TurboFac;

namespace TurboFacTests
{
	[TestClass]
	public static partial class _AssemblyInit
	{
		public static ReflectionPermission ReflectionPermission;

		[AssemblyInitialize]
		public static void Init(TestContext ctx)
		{
			TurboContainer.DefaultReflectionPermission =
			ReflectionPermission = typeof(_AssemblyInit).Assembly.FullName.Contains("Precompiled")
				? (Debugger.IsAttached
					? ReflectionPermission.Throw
					: ReflectionPermission.Throw)
				: ReflectionPermission.Allow;
			Init(TurboContainer.Root);
		}

		static partial void Init(ITurboContainer c);

		public static void InitContainer(ITurboContainer c)
		{
			Init(c);
		}
	}
}
