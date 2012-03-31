using System.Collections.Generic;
using System;
using System.Linq;

using MetaCreator;
using MetaCreator.Extensions;

using TurboFac.ThirdParty;

//namespace TurboFac.MetaRegister
//{
namespace TurboFac.Meta
{
	public static class TurboFacMetaRegisterExtension
	{
		public static void RegisterAll(this IMetaWriter writer)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
				.Where(x =>
				       !x.FullName.StartsWith("System")
				       && !x.FullName.StartsWith("Microsoft.")
				       && !x.FullName.StartsWith("mscorlib")
				       && !x.FullName.StartsWith("MetaCreator,")
				       && !x.FullName.StartsWith("TurboFac,")
				       && !x.FullName.StartsWith("Accessibility,")
				       && !x.IsDynamic // not sure
				);

#if DEBUG
			foreach (var assembly in assemblies)
			{
				writer.WriteLine("// "+assembly.FullName);
			}
#endif

			var types = assemblies.SelectMany(x => x.GetTypes()).Where(x => x.Attribute<TurboRegAttribute>() != null);

			foreach (var type in types)
			{
				// writer.WriteLine("// " + type.FullName);
				var ifaces = TurboContainer.GetAdditionalIfaces(type);
				foreach (var iface in ifaces)
				{
					var ctor = TurboContainer.GetConstructor(type);
					var arguments = string.Join(", ", ctor.GetParameters().Select(x => string.Format("c.Get<{0}>()", x.ParameterType.CSharpTypeIdentifier())));
					writer.WriteLine("c.Add(new Lazy<{0}>(()=>new {1}({2})));", iface.CSharpTypeIdentifier(), type.CSharpTypeIdentifier(), arguments);
				}
			}
		}
	}
}
//}
