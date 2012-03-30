using System;
using System.Reflection;

#if NET3
using MyUtils;
#endif

namespace TurboFac
{
	/// <summary>
	/// Convert Lazy&lt;object&gt; to &lt;T&gt; with T provided as Type
	/// </summary>
	public class TypedLazyWrapper
	{
		// TODO optimize reflection!
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
	}
}