using System;
using System.ComponentModel;

#if NET3
using MyUtils;
#endif

namespace TurboFac
{
	/// <summary>
	/// TurboFac Service Container
	/// </summary>
	[DefaultImpl(typeof(TurboContainer))]
	public interface ITurboContainer : ITurboProvider , IDisposable
	{
#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		void Add(Type type);

#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		void Add(Type type, Type implementation);

#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		void Add(Type type, Lazy<object> instanceFactory);
	}
}