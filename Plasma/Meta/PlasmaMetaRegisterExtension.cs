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

		private static SharpGenerator.TypeIdentifierConfig _omitTypeDef = new SharpGenerator.TypeIdentifierConfig
		{
			UseNamedTypeParameters = false,
		};

		private static bool _enableNullObject;
		private static bool _enableProxy;

		/// <summary>
		/// Enable or disable NullObject feature
		/// </summary>
		public static void PlasmaNullObject(this IMetaWriter writer, bool enabled)
		{
			_enableNullObject = enabled;
		}

		/// <summary>
		/// Enable or disable NullObject feature
		/// </summary>
		public static void PlasmaNullObject(this IMetaWriter writer, string enabled)
		{
			PlasmaNullObject(writer, bool.Parse(enabled));
		}

		/// <summary>
		/// Enable or disable Proxy feature
		/// </summary>
		public static void PlasmaProxy(this IMetaWriter writer, bool enabled)
		{
			_enableProxy = enabled;
		}

		/// <summary>
		/// Enable or disable Proxy feature
		/// </summary>
		public static void PlasmaProxy(this IMetaWriter writer, string enabled)
		{
			PlasmaProxy(writer, bool.Parse(enabled));
		}

		/// <summary>
		/// Finally write all code
		/// End your file with that line
		/// </summary>
		public static void PlasmaGenerate(this IMetaWriter writer)
		{
			var types = _context.Types;

			#region Proxy

			if (_enableProxy)
			{
				foreach (var type in types.Where(x => x.IsInterface))
				{
					_proxyGenerator.Generate(writer, type);
					// GenerateStub(writer, type);
				}
			}

			#endregion

			#region Null

			if (_enableNullObject)
			{
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
			}

			#endregion

			writer.WriteLine(@"
public static partial class PlasmaRegistration
{
	public static void Run()
	{
			Plasma.PlasmaContainer.DefaultReflectionPermission = Plasma.ReflectionPermission.Throw;
");
			writer.WriteLine(); // ignore tabs

			var typeToPlumb = types.ToList();

			foreach (var type in types.Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition && x.IsPublic))
			{
				try
				{
					writer.WriteLine("Plasma.Internal.TypeFactoryRegister.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.CreateType(type));
				}
				catch (StaticCompilerWarning ex)
				{
					writer.WriteLine("#warning " + ex.Message);
				}
			}

			writer.WriteLine();
			writer.WriteLine("// Iface impl");

			foreach (var type in types)
			{
				if (type.IsInterface)
				{
					Type defImpl = null;
					try
					{
						defImpl = Mining.IfaceImpl(type);
					}
					catch (Exception ex)
					{
						writer.WriteLine("// " + ex.Message);
					}

					if (defImpl != null)
					{
						writer.WriteLine("Plasma.Internal.FaceImplRegister.Register<{0}, {1}>();", type, defImpl);
					}
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
{1}}});", type.CSharpTypeIdentifier(_omitTypeDef), plumbing);
					}
					else
					{
						writer.WriteLine("Plasma.Internal.TypeAutoPlumberRegister.RegisterNone(typeof({0}));", type.CSharpTypeIdentifier(_omitTypeDef));
					}
				}
			}
			if (_enableNullObject)
			{
				foreach (var type in _nullGenerator.NullObjects.Concat(_nullGenerator.ExplicitRequests).Distinct())
				{
					if (type.IsGenericType && type.IsGenericTypeDefinition)
					{
						writer.WriteLine(@"Null.RegisterGeneric(typeof({0}), t =>
	typeof ({1}).MakeGenericType(t).GetField(""Instance"").GetValue(null));", type.CSharpTypeIdentifier(new SharpGenerator.TypeIdentifierConfig
						{
							UseEngineImports = true,
							UseNamedTypeParameters = false,
						}), _nullGenerator.GetTypeName(type).FullNameGenericNoT);

						//typeof (NullEnumerable<>).MakeGenericType(t).GetField("Instance").GetValue(null));

					}
					else
					{
						writer.WriteLine("Null.Register<{0}>({1}.Instance);", type.CSharpTypeIdentifier(), _nullGenerator.GetTypeName(type).FullNameGeneric);
					}
				}
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
					   && !x.FullName.StartsWith("MetaCreator")
					   && !x.FullName.StartsWith("Plasma,")
					   && !x.FullName.StartsWith("Plasma ")
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
				/*&& (
				x.Attribute2<RegisterServiceAttribute>() != null
				|| x.IsInterface
				)*/
				//|| x.Attribute2<PlasmaServiceAttribute>() != null
				//|| x.Attribute<DefaultImplAttribute>() != null
);


			writer.PlasmaRegisterType(types.ToArray());
		}

		public static void PlasmaRegisterType(this IMetaWriter writer, params Type[] types)
		{
			foreach (var type in types)
			{
				PlasmaRegisterType(writer, type);
			}
		}

		public static void PlasmaRegisterType(this IMetaWriter writer, Type type)
		{
			if (PlasmaContainer.IsValidImplementationType(type))
			{
				writer.WriteLine("// " + type.CSharpTypeIdentifier());
				_context.Types.Add(type);
			}
		}

		public static void PlasmaRegisterType<T>(this IMetaWriter writer)
		{
			PlasmaRegisterType(writer, typeof (T));
		}

		static readonly ClassGeneratorStrategy _proxyGenerator = new ProxyClassGeneratorStrategy();
		static readonly NullObjectClassGeneratorStrategy _nullGenerator = new NullObjectClassGeneratorStrategy();
	}
}

