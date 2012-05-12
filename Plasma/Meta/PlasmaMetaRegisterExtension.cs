using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

using MetaCreator;

using Plasma.ThirdParty;

namespace Plasma.Meta
{
	public static class PlasmaMetaRegisterExtension
	{
		static readonly Mining Mining = new StaticMining();

		public static void RegisterAll(this IMetaWriter writer)
		{
			writer.WriteLine(); // ignore tabs

			// todo - fetch external references!!
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
				.Where(x =>
				       !x.Location.ToLowerInvariant().StartsWith(Path.GetTempPath().ToLowerInvariant())
				       && !x.FullName.StartsWith("System")
				       && !x.FullName.StartsWith("Microsoft.")
				       && !x.FullName.StartsWith("mscorlib")
				       && !x.FullName.StartsWith("MetaCreator,")
				       && !x.FullName.StartsWith("Plasma,")
				       && !x.FullName.StartsWith("Accessibility,")
				       && !x.IsDynamic // not sure
				);

#if DEBUG
			foreach (var assembly in assemblies)
			{
				writer.WriteLine("// " + assembly.FullName); // + " IsDynamic:" + assembly.IsDynamic + " HostContext:" + assembly.HostContext + " Location:" + assembly.Location);
			}
#endif

			var types = assemblies.SelectMany(x => x.GetTypes())
				//.Where(x =>
				// x.Attribute<RegisterServiceAttribute>() != null ||
				//x.Attribute<PlasmaServiceAttribute>() != null ||
				//x.Attribute<DefaultImplAttribute>() != null)
				.ToArray();

			var typeToPlumb = types.ToList();

			foreach (var type in types)
			{
				// writer.WriteLine("// " + type.FullName);
//				var ifaces = Mining.GetAdditionalIfaces(type);
//				foreach (var iface in ifaces)
//				{
//					//					var ci = Mining.GetConstructor(type);
//					//					var arguments = string.Join(", ", Mining.GetConstructorArguments(ci));
//					//					// todo remove dependancy on 'c'
//					//					writer.WriteLine("c_.Add<{0}>(()=>new {1}({2}));", iface.CSharpTypeIdentifier(), type.CSharpTypeIdentifier(), arguments);
//
//					writer.WriteLine("Plasma.Internal.TypeFactoryRegister1.Add<{0}>({1});", iface.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
//				}
//				var def = type.Attribute<DefaultImplAttribute>();
//				if ((type.IsInterface || type.IsAbstract) && def != null)
//				{
//					typeToPlumb.Add(def.TargetType);
//					writer.WriteLine("Plasma.Internal.TypeFactoryRegister2.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.DefaultFactory(def.TargetType));
//				}
//				if (ifaces.Count() == 0 /*&& type.Attribute<PlasmaServiceAttribute>() != null*/)
				if (!type.IsAbstract && !type.IsInterface && !type.IsGenericType && type.IsPublic)
				{
					//					var ci = Mining.GetConstructor(type);
					//					var arguments = string.Join(", ", Mining.GetConstructorArguments(ci));
					//					// todo remove dependancy on 'c'
					//					writer.WriteLine("c_.Add(()=>new {1}({2}));", null, type.CSharpTypeIdentifier(), arguments);

					writer.WriteLine("Plasma.Internal.TypeFactoryRegister.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
				}
			}
			writer.WriteLine();
			writer.WriteLine("// Property injectors optimization");
			foreach (var type in typeToPlumb)
			{
				if (type.IsClass && !type.IsAbstract)
				{
					var plumbingPros = PlasmaContainer.GetPlumbingProperties(type).ToArray();
					if (plumbingPros.Length > 0)
					{
						var plumbing = "";
						foreach (var pi in plumbingPros)
						{
							plumbing += string.Format("\tx.{0} = {1};\r\n", pi.Name, Mining.GetArgument(pi));
						}
						// todo remove extra cast in plumber
						// todo remove dependancy on 'c'
						writer.WriteLine(@"Plasma.Internal.TypeAutoPlumberRegister.Register<{0}>((c, x)=>{{
{1}}});", type.CSharpTypeIdentifier(), plumbing);
					}
					else
					{
						writer.WriteLine("Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<{0}>();", type.CSharpTypeIdentifier());
					}
				}
			}
		}


	}
}

