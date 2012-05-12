using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class PlasmaServiceAttribute : Attribute
	{
		
	}

	/// <summary>
	/// Mark classes for automatic registration
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		public sealed class RegisterServiceAttribute : PlasmaServiceAttribute
	{
		readonly Type[] _asInterfaces;

		public RegisterServiceAttribute()
		{
			
		}

		public RegisterServiceAttribute(params Type[] asInterfaces)
		{
			_asInterfaces = asInterfaces;
		}

		public Type[] AsInterfaces
		{
			get { return _asInterfaces; }
		}
	}
}
