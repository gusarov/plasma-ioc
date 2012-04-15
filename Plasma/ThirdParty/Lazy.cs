using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Plasma.ThirdParty
{
	static class Lazy
	{
		public static Lazy<T> New<T>(Func<T> factory) where T : class
		{
			return new Lazy<T>(factory);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static Lazy<T> New<T>() where T : class, new()
		{
			return new Lazy<T>(Activator.CreateInstance<T>);
		}

	}

	class Lazy<T>
	{
		Func<T> _factory;
		T _value;
		readonly object _factorySync = new object();

		public Lazy(Func<T> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			_factory = factory;
		}

		public Lazy(T value)
		{
			_value = value;
		}


		public T Value
		{
			get
			{
				if (_factory != null)
				{
					lock (_factorySync)
					{
						if (_factory != null)
						{
							_value = _factory();
							_factory = null;
						}
					}
				}
				return _value;
			}
		}

		public bool IsValueCreated
		{
			get { return _factory == null; }
		}

	}
}