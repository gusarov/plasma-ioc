using System.Linq;
using System.Collections.Generic;
using System;

namespace TurboFac
{
	public enum ReflectionPermission
	{
		Unknown,
		Allow,
		Log,
		DebuggerBreakIfAttached,
		DebugAssertion,
		Throw,
	}
}