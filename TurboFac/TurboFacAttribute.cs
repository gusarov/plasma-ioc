using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurboFac
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class TurboFacAttribute : Attribute
	{
	}
}
