using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel;

using Plasma.ThirdParty;

#if NET3
using MyUtils;
#endif

namespace Plasma
{
	/// <summary>
	/// Plasm Service Provider
	/// </summary>
	[DefaultImpl(typeof(PlasmaContainer))]
	public interface IPlasmaProvider
	{
#if !PocketPC
		/// <summary>
		/// Get a service of registered type
		/// </summary>
		/// <exception cref="PlasmaException">Service is not registered</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		object Get(Type type);

#if !PocketPC
		/// <summary>
		/// Get a lazy service of registered type
		/// </summary>
		/// <exception cref="PlasmaException">Service is not registered</exception>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		Lazy<object> GetLazy(Type type);

#if !PocketPC
		/// <summary>
		/// Try get service of registered type and return null on failure
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		object TryGet(Type type);

#if !PocketPC
		/// <summary>
		/// Try get lazy service of registered type and return null on failure
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		Lazy<object> TryGetLazy(Type type);
	}
}