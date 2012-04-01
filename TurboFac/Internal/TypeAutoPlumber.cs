using System;
using System.Collections.Generic;
using System.Linq;

namespace TurboFac.Internal
{
	using Key = Action<ITurboContainer, object>;

	/// <summary>
	/// Register of plumbing code - property injectors
	/// </summary>
	public static class TypeAutoPlumberRegister
	{
		static readonly Dictionary<Type, Key> _plumbers = new Dictionary<Type, Key>();

		/// <summary>
		/// Add an action for property injection
		/// </summary>
		public static void Register<T>(Action<ITurboContainer, T> action)
		{
			_plumbers[typeof(T)] = (c, x) => action(c, (T)x);
		}

		/// <summary>
		/// Add an action for property injection
		/// </summary>
		public static void Register<T>(Key action)
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
		internal static bool TryPlumb(object instance, ITurboContainer c)
		{
			Key act;
			if (_plumbers.TryGetValue(instance.GetType(), out act))
			{
				act(c, instance);
				return true;
			}
			return false;
		}
	}
}
