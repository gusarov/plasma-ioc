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
			writer.WriteLine(@"
public static class PlasmaRegistration
{
	public static void Run()
	{
");
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

			foreach (var type in types.Where(x =>!x.IsAbstract && !x.IsInterface && !x.IsGenericType && x.IsPublic))
			{
				writer.WriteLine("Plasma.Internal.TypeFactoryRegister.Add<{0}>({1});", type.CSharpTypeIdentifier(false), Mining.DefaultFactory(type));
			}

			foreach (var type in types)
			{
				continue;
				// writer.WriteLine("// " + type.FullName);
				var ifaces = Mining.GetAdditionalIfaces(type);
				foreach (var iface in ifaces)
				{
					//					var ci = Mining.GetConstructor(type);
					//					var arguments = string.Join(", ", Mining.GetConstructorArguments(ci));
					//					// todo remove dependancy on 'c'
					//					writer.WriteLine("c_.Add<{0}>(()=>new {1}({2}));", iface.CSharpTypeIdentifier(), type.CSharpTypeIdentifier(), arguments);

					writer.WriteLine("Plasma.Internal.TypeFactoryRegister1.Add<{0}>({1});", iface.CSharpTypeIdentifier(false), Mining.DefaultFactory(type));
				}
				var def = type.Attribute<DefaultImplAttribute>();
				if ((type.IsInterface || type.IsAbstract) && def != null)
				{
					typeToPlumb.Add(def.TargetType);
					writer.WriteLine("Plasma.Internal.TypeFactoryRegister2.Add<{0}>({1});", type.CSharpTypeIdentifier(false), Mining.DefaultFactory(def.TargetType));
				}
				if (ifaces.Count() == 0 /*&& type.Attribute<PlasmaServiceAttribute>() != null*/)
				{
					//					var ci = Mining.GetConstructor(type);
					//					var arguments = string.Join(", ", Mining.GetConstructorArguments(ci));
					//					// todo remove dependancy on 'c'
					//					writer.WriteLine("c_.Add(()=>new {1}({2}));", null, type.CSharpTypeIdentifier(), arguments);

					writer.WriteLine("Plasma.Internal.TypeFactoryRegister.Add<{0}>({1});", type.CSharpTypeIdentifier(false), Mining.DefaultFactory(type));
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
{1}}});", type.CSharpTypeIdentifier(false), plumbing);
					}
					else
					{
						writer.WriteLine("Plasma.Internal.TypeAutoPlumberRegister.RegisterNone<{0}>();", type.CSharpTypeIdentifier(false));
					}
				}
			}

			writer.WriteLine(@"
	}
}

");

			foreach (var type in types.Where(x => x.IsInterface))
			{
				var methods = type.GetMethods().Where(x => !x.IsSpecialName);

				if (!methods.Any())
				{
					continue;
				}

				var name = type.Name.StartsWith("I") ? type.Name.Substring(1) : type.Name;

				writer.WriteLine("public class Proxy{0} : Plasma.Proxy.ProxyBase<{1}>", name, type.CSharpTypeIdentifier(false));
				writer.WriteLine("{");
				writer.WriteLine("\tpublic Proxy{0}({1} originalObject) : base(originalObject)", name, type.CSharpTypeIdentifier(false));
				writer.WriteLine("\t{");
				writer.WriteLine("\t}");

				foreach (var method in methods)
				{
					writer.WriteLine("\tpublic virtual {0} {1}{2}({3})", method.ReturnType.CSharpTypeIdentifier(false), method.Name, null, string.Join(", ", method.GetParameters().Select(x => x.ParameterType.CSharpTypeIdentifier(false) + " " + x.Name)));
					writer.WriteLine("\t{");
					writer.WriteLine("\t\t{0}Original.{1}({2});", method.ReturnType == typeof(void) ? "" : "return ", method.Name, string.Join(", ", method.GetParameters().Select(x => x.Name)));
					writer.WriteLine("\t}");
				}

				writer.WriteLine("}");
			}
		}


	}
}

