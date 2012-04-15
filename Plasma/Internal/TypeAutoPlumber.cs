using System;
using System.Collections.Generic;
using System.Linq;

namespace Plasma.Internal
{
	/// <summary>
	/// Register of plumbing code - property injectors
	/// </summary>
	public static class TypeAutoPlumberRegister
	{
		static readonly Dictionary<Type, Action<IPlasmaContainer, object>> _plumbers = new Dictionary<Type, Action<IPlasmaContainer, object>>();

		/// <summary>
		/// Add an action for property injection
		/// </summary>
		public static void Register<T>(Action<IPlasmaContainer, T> action)
		{
			_plumbers[typeof(T)] = (c, x) => action(c, (T)x);
		}

		/// <summary>
		/// Add an action for property injection
		/// </summary>
		public static void Register<T>(Action<IPlasmaContainer, object> action)
		{
			_plumbers[typeof(T)] = action;
		}

		/// <summary>
		/// Express the fact that no property injection exists for this type
		/// </summary>
		public static void RegisterNone<T>()
		{
			_plumbers[typeof(T)] = delegate { }; // todo remove extra delegate instance
		}

		/// <summary>
		/// Inject dependencies from container
		/// </summary>
		internal static bool TryPlumb(object instance, IPlasmaContainer c)
		{
			Action<IPlasmaContainer, object> act;
			if (_plumbers.TryGetValue(instance.GetType(), out act))
			{
				act(c, instance);
				return true;
			}
			return false;
		}
	}
}
