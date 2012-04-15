using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma
{
	/// <summary>
	/// Suggest this constructor for automatic registration or instantiating
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
	public sealed class DefaultConstructorAttribute : Attribute
	{
		
	}
}