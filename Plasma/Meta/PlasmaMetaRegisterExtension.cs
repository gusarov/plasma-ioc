using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
#if !NET3
				       && !x.IsDynamic // not sure
#endif
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
				GenerateProxy(writer, type);
				GenerateStub(writer, type);
				GenerateNullObject(writer, type);
			}
		}

		static void GenerateNullObject(IMetaWriter writer, Type type)
		{
			
		}

		static void GenerateStub(IMetaWriter writer, Type type)
		{
			
		}

		static void GenerateProxy(IMetaWriter writer, Type type)
		{
//			writer.WriteLine("// iface - " + type.Name);
//			writer.WriteLine("// all ifaces - " + string.Join(", ", Mining.GetAllIfaces(type).Distinct().Select(x => x.Name)));

			var members = type.GetMembers().Concat(Mining.GetAllIfaces(type).Distinct().SelectMany(x => x.GetMembers())).ToArray();

			if (!members.Any())
			{
				return;
			}

			var name = type.Name.StartsWith("I") ? type.Name.Substring(1) : type.Name;

			writer.WriteLine("public class Proxy{0} : Plasma.Proxy.ProxyBase<{1}>, {1}", name, type.CSharpTypeIdentifier(false));
			writer.WriteLine("{");

			writer.Write("\tpublic Proxy{0}({1} originalObject) : base(originalObject)", name, type.CSharpTypeIdentifier(false));
			writer.Write("\t{");
			writer.WriteLine("\t}");

			foreach (var method in members.Where(x => x.MemberType == MemberTypes.Method).Cast<MethodInfo>().Where(x=>!x.IsSpecialName))
			{
				writer.Write("\tpublic virtual {0} {1}{2}({3})", method.ReturnType.CSharpTypeIdentifier(false), method.Name, null, string.Join(", ", method.GetParameters().Select(x => x.ParameterType.CSharpTypeIdentifier(false) + " " + x.Name)));
				writer.Write(" { ");
				writer.Write("{0}Original.{1}({2});", method.ReturnType == typeof(void) ? "" : "return ", method.Name, string.Join(", ", method.GetParameters().Select(x => x.Name)));
				writer.WriteLine(" }");
			}

			foreach (var eventInfo in members.Where(x => x.MemberType == MemberTypes.Event).Cast<EventInfo>())
			{
				writer.Write("\tpublic virtual event {0} {1}", eventInfo.EventHandlerType.CSharpTypeIdentifier(false), eventInfo.Name);
				writer.Write(" { ");
				writer.Write(" add { ");
				writer.Write("Original.{0} += value;", eventInfo.Name);
				writer.Write(" }");
				writer.Write(" remove { ");
				writer.Write("Original.{0} -= value;", eventInfo.Name);
				writer.Write(" }");
				writer.WriteLine(" }");
			}

			foreach (var pro in members.Where(x => x.MemberType == MemberTypes.Property).Cast<PropertyInfo>())
			{
				var isIndexer = pro.GetIndexParameters().Any();

				var indexerParameters = string.Join(", ", pro.GetIndexParameters().Select(x => x.ParameterType.CSharpTypeIdentifier() + " " + x.Name));
				indexerParameters = indexerParameters.Any() ? " [" + indexerParameters + "]" : null;

				var indexerArguments = string.Join(", ", pro.GetIndexParameters().Select(x => x.Name));
				indexerArguments = indexerArguments.Any() ? "[" + indexerArguments + "]" : null;

				var myProName = isIndexer ? "this" : pro.Name;
				var orProName = isIndexer ? "" : "."+pro.Name;

				writer.Write("\tpublic virtual {0} {1}{2}", pro.PropertyType.CSharpTypeIdentifier(false), myProName, indexerParameters);
				writer.Write(" { ");
				if (pro.CanRead)
				{
					writer.Write(" get { ");
					writer.Write("return Original{0}{1};", orProName, indexerArguments);
					writer.Write(" }");
				}
				if (pro.CanWrite)
				{
					writer.Write(" set { ");
					writer.Write("Original{0}{1} = value;", orProName, indexerArguments);
					writer.Write(" }");
				}
				writer.WriteLine(" }");
			}

			writer.WriteLine("}");
		}
	}
}

