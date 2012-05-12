using System;
using System.Collections.Generic;

namespace Plasma.Internal
{
	/// <summary>
	/// Register of factory code
	/// </summary>
	public static class TypeFactoryRegister
	{
		static readonly Dictionary<Type, Func<IPlasmaContainer, object>> _factories = new Dictionary<Type, Func<IPlasmaContainer, object>>();

		public static void Add<T>(Func<IPlasmaContainer, object> instanceFactory)
		{
			_factories[typeof(T)] = instanceFactory;
		}

		internal static object Create(IPlasmaContainer container, Type type)
		{
			Func<IPlasmaContainer, object> factory;
			if (_factories.TryGetValue(type, out factory))
			{
				return factory(container);
			}
			return null;
		}
	}
}