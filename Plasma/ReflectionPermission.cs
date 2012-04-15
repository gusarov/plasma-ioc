using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma
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