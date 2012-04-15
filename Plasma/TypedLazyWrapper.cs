using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

#if NET3
using MyUtils;
#endif

namespace Plasma
{
	/// <summary>
	/// Convert Lazy&lt;object&gt; to &lt;T&gt; with T provided as Type
	/// </summary>
	internal class TypedLazyWrapper
	{
		static readonly MethodInfo _method = typeof(TypedLazyWrapper).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

		readonly Lazy<object> _lazyObject;
		public object Lazy { get; private set; }

		public TypedLazyWrapper(Type lazyParameterType, Lazy<object> lazyObject)
		{
			_lazyObject = lazyObject;
			_method.MakeGenericMethod(lazyParameterType).Invoke(this, null);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		protected void Init<T>()
		{
			Lazy = new Lazy<T>(() => (T)_lazyObject.Value);
		}

		internal static object Create(Type type, Lazy<object> serviceLazy)
		{
			// TODO optimize reflection!
			Func<Lazy<object>, object> typedLazyFactory;
			if (_typedLazyFactory.TryGetValue(type, out typedLazyFactory))
			{
				return typedLazyFactory(serviceLazy);
			}
			return new TypedLazyWrapper(type, serviceLazy).Lazy;
		}

		static readonly Dictionary<Type, Func<Lazy<object>, object>> _typedLazyFactory = new Dictionary<Type, Func<Lazy<object>, object>>();

		public static void Register<T>(Type type, Func<Lazy<object>, Lazy<T>> factory)
		{
			_typedLazyFactory[type] = factory;
		}
	}

	internal class TypedFuncWrapper
	{
		static readonly MethodInfo _method = typeof(TypedFuncWrapper).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

		readonly Lazy<object> _lazyObject;
		public object Func { get; private set; }

		public TypedFuncWrapper(Type lazyParameterType, Lazy<object> lazyObject)
		{
			_lazyObject = lazyObject;
			_method.MakeGenericMethod(lazyParameterType).Invoke(this, null);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		protected void Init<T>()
		{
			Func = new Func<T>(() => (T)_lazyObject.Value);
		}

		internal static object Create(Type type, Lazy<object> serviceLazy)
		{
			// TODO optimize reflection!
			Func<Lazy<object>, object> typedLazyFactory;
			if (_typedLazyFactory.TryGetValue(type, out typedLazyFactory))
			{
				return typedLazyFactory(serviceLazy);
			}
			return new TypedFuncWrapper(type, serviceLazy).Func;
		}

		static readonly Dictionary<Type, Func<Lazy<object>, object>> _typedLazyFactory = new Dictionary<Type, Func<Lazy<object>, object>>();

		public static void Register<T>(Type type, Func<Lazy<object>, Func<T>> factory)
		{
			_typedLazyFactory[type] = factory;
		}
	}
}