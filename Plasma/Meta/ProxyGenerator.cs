using System.Linq;
using System.Collections.Generic;
using System;

using MetaCreator;

namespace Plasma.Meta
{
	/// <summary>
	/// 
	/// </summary>
	public static class ProxyGenerator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="typeName"></param>
		public static void UniversalProxy(this IMetaWriter writer, string typeName)
		{
			var type = Type.GetType(typeName);

		}
	}
}
