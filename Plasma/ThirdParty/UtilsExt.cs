using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Plasma.ThirdParty
{
	static class UtilsExt
	{
		public static T Attribute2<T>(this ICustomAttributeProvider cap) where T : Attribute
		{
			var atrs = cap.GetCustomAttributes(typeof(T), true);
			if (atrs.Length > 0)
			{
				return (T) atrs[0];
			}
			return null;
		}
	}
}