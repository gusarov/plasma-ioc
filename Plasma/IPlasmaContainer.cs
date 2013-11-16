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
	/// Plasma Service Container
	/// </summary>
	[DefaultImpl(typeof(PlasmaContainer))]
	public interface IPlasmaContainer : IPlasmaProvider , IDisposable
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