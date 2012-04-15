using System;
using System.Collections.Generic;
using System.Linq;

namespace Plasma.Aop
{
	/// <summary>
	/// Specify aspect advisor
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class AspectAttribute : Attribute
	{
		readonly Type _advise;

		/// <summary>
		/// Advisor implementatino type
		/// </summary>
		public Type Advise
		{
			get { return _advise; }
		}

		/// <summary>
		/// Specify aspect advisor
		/// </summary>
		public AspectAttribute(Type advise)
		{
			_advise = advise;
		}
	}
}
