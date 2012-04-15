using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma.Aop
{
	/// <summary>
	/// Aspect for caching method calculations
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class CacheAttribute : AspectAttribute
	{
		/// <summary>
		/// Aspect for caching method calculations
		/// </summary>
		public CacheAttribute() : base(typeof(CacheAdvise)) {}
	}
}