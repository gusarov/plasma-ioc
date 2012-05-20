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
		class Context
		{
			public readonly HashSet<Type> Types = new HashSet<Type>();
		}

		static Context _context = new Context();

		static readonly Mining Mining = new StaticMining();

		/// <summary>
		/// Clear generation context
		/// Start your file from that line
		/// </summary>
		public static void PlasmaClear(this IMetaWriter writer)
		{
			_context = new Context();
		}

		/// <summary>
		/// Finally write all code
		/// End your file with that line
		/// </summary>
		public static void PlasmaGenerate(this IMetaWriter writer)
		{
			var types = _context.Types;

			#region Proxy

			foreach (var type in types.Where(x => x.IsInterface))
			{
				_proxyGenerator.Generate(writer, type);
				// GenerateStub(writer, type);
			}

			#endregion

			#region Null

			var req = types.Where(x => x.IsInterface);
			do
			{
				foreach (var type in req.ToArray())
				{
					_nullGenerator.Generate(writer, type);
				}
				req = _nullGenerator.RequestedObjects.Except(_nullGenerator.NullObjects).ToArray();
				_nullGenerator.RequestedObjects.Clear();
			} while (req.Any());

			#endregion

			writer.WriteLine(@"
public static partial class PlasmaRegistration
{
	public static void Run()
	{
");
			writer.WriteLine(); // ignore tabs

			var typeToPlumb = types.ToList();

			foreach (var type in types.Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition && x.IsPublic))
			{
				writer.WriteLine("Plasma.Internal.TypeFactoryRegister.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
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

					writer.WriteLine("Plasma.Internal.TypeFactoryRegister1.Add<{0}>({1});", iface.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
				}
				var def = type.Attribute2<DefaultImplAttribute>();
				if ((type.IsInterface || type.IsAbstract) && def != null)
				{
					typeToPlumb.Add(def.TargetType);
					writer.WriteLine("Plasma.Internal.TypeFactoryRegister2.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.DefaultFactory(def.TargetType));
				}
				if (ifaces.Count() == 0 /*&& type.Attribute<PlasmaServiceAttribute>() != null*/)
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
			foreach (var type in _nullGenerator.NullObjects.Concat(_nullGenerator.ExplicitRequests).Distinct())
			{
				//writer.WriteLine("Null.Register<{0}>({1}.Instance);", type.CSharpTypeIdentifier(), _nullGenerator.GetTypeName(type).FullNameGeneric);
			}
			writer.WriteLine(@"
	}
}

");




		}

		public static void PlasmaRegisterAll(this IMetaWriter writer)
		{
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
			PlasmaRegisterAssembly(writer, assemblies.ToArray());
		}

		public static void PlasmaRegisterAssembly<T>(this IMetaWriter writer)
		{
			PlasmaRegisterAssembly(writer, typeof(T));
		}

		public static void PlasmaRegisterAssembly(this IMetaWriter writer, params Type[] types)
		{
			PlasmaRegisterAssembly(writer, types.Select(x => x.Assembly).ToArray());
		}

		public static void PlasmaRegisterAssembly(this IMetaWriter writer, params Assembly[] asms)
		{
			var types = asms.SelectMany(x => x.GetTypes())
.Where(x =>
x.IsPublic
				// x.Attribute<RegisterServiceAttribute>() != null
				//|| x.Attribute<PlasmaServiceAttribute>() != null
				//|| x.Attribute<DefaultImplAttribute>() != null
);

			foreach (var type in types)
			{
				_context.Types.Add(type);
			}
		}

		static readonly ClassGeneratorStrategy _proxyGenerator = new ProxyClassGeneratorStrategy();
		static readonly NullObjectClassGeneratorStrategy _nullGenerator = new NullObjectClassGeneratorStrategy();
	}
}

