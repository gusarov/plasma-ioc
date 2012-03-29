using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MetaCreator;
using MetaCreator.Extensions;

using MySpring;

namespace TurboFac
{
	public static class TurboFacGenerator
	{
		public static void RegisterAll(this IMetaWriter writer/*, ISpringContainer container*/)
		{
			var typesAll = AppDomain.CurrentDomain.GetAssemblies();//.SelectMany(x => x.GetTypes());

			foreach (var type in typesAll)
			{
				writer.WriteLine("// "+type.FullName);
				// writer.WriteLine("// c.Add<{0}>();", MixinExtension.CSharpTypeIdentifier(type));
			}

			var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => UtilsExt.Attribute<TurboFacAttribute>(x) != null);
			foreach (var type in types)
			{
				writer.WriteLine("c.Add<{0}>();", MixinExtension.CSharpTypeIdentifier(type));
			}
		}
	}
}
