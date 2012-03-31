using System;

namespace TurboFac
{
	/// <summary>
	/// Suggest this constructor for automatic registration or instantiating
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
	public sealed class DefaultConstructorAttribute : Attribute
	{
		
	}
}