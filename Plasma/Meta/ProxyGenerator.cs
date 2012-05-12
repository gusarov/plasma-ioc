using System.Linq;
using System.Collections.Generic;
using System;

using MetaCreator;

namespace Plasma.Meta
{
	public static class ProxyGenerator
	{
		public static void UniversalProxy(this IMetaWriter writer, string typeName)
		{
			var type = Type.GetType(typeName);
			
		}
	}
}
