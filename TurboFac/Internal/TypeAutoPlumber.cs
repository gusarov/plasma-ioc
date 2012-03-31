using System;
using System.Collections.Generic;
using System.Linq;

namespace TurboFac.Internal
{
	public static class TypeAutoPlumberRegister
	{
		static readonly Dictionary<Type, Action<object>> _plumbers = new Dictionary<Type, Action<object>>();

		public static void Register<T>(Action<object> action)
		{
			_plumbers[typeof(T)] = action;
		}

		public static void RegisterNone<T>()
		{
			_plumbers[typeof(T)] = delegate { }; // todo remove extra delegate instance
		}

		public static bool TryPlumb(object instance, ITurboContainer c)
		{
			Action<object> act;
			if (_plumbers.TryGetValue(instance.GetType(), out act))
			{
				act(instance);
				return true;
			}
			return false;
		}
	}
}
