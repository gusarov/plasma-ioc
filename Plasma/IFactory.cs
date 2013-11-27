using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plasma
{
	/// <summary>
	/// This interface allows to easily expose custom factory for PlasmaContainer.
	/// Note that IFactory&lt;TI, TR&gt; is also required
	/// </summary>
	public interface IFactory
	{
/*
		/// <summary>
		/// Create an object
		/// </summary>
		object Create();

		/// <summary>
		/// Specify target type for factory
		/// </summary>
		Type TargetType { get; }
*/
	}

	/// <summary>
	/// This interface allows to easily expose custom factory for PlasmaContainer
	/// </summary>
	public interface IFactory<out TI, TR> : IFactory where TR : TI
	{
		/// <summary>
		/// Create an object
		/// </summary>
		TI Create();
	}

	/// <summary>
	/// Custom factory for specified type
	/// </summary>
	public abstract class Factory<TI, TR> : IFactory<TI, TR> where TR : TI
	{
/*		/// <summary>
		/// Create an object
		/// </summary>
		object IFactory.Create()
		{
			return Create();
		}*/

		/// <summary>
		/// Create an object
		/// </summary>
		public abstract TI Create();

/*		/// <summary>
		/// Specify target type for factory
		/// </summary>
		public Type TargetType
		{
			get { return typeof(TI); }
		}*/
	}
}
