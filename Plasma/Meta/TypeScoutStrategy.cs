using System.Linq;
using System.Collections.Generic;
using System;
using MetaCreator;

namespace Plasma.Meta
{
	abstract class TypeScoutStrategy
	{
		/// <summary>
		/// Predicate that determines whether a strategy going to work with this type
		/// </summary>
		public abstract bool Filter(Type type);

		/// <summary>
		/// Analyze type and yield all required type requests that are supposed to be directed to container
		/// </summary>
		public abstract IEnumerable<Type> GetRequestsCore(Type type);

		/// <summary>
		/// Analyze each type and yield all required type requests that are supposed to be directed to container
		/// </summary>
		public IEnumerable<Type> GetRequests(IEnumerable<Type> types)
		{
			return types.Where(Filter).SelectMany(GetRequestsCore);
		}

		/// <summary>
		/// Generate code necessary to prepare registers for working with types collected
		/// </summary>
		public abstract void Write(IMetaWriter writer);
	}
}