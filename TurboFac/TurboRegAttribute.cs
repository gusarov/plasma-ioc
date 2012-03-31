using System;

namespace TurboFac
{
	/// <summary>
	/// Mark classes for automatic registration
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class RegisterServiceAttribute : Attribute
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
