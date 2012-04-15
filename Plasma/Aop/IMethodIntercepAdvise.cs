using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma.Aop
{
	/// <summary>
	/// Advisor for method invokation
	/// </summary>
	public interface IMethodIntercepAdvise : IAdvise
	{
		/// <summary>
		/// Extension point for invokation
		/// </summary>
		object Invoke(Func<object[], object> body, object[] args);
	}
}