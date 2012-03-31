﻿using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

using MetaCreator;
using MetaCreator.Extensions;

using TurboFac.ThirdParty;

//namespace TurboFac.MetaRegister
//{
namespace TurboFac.Meta
{
	public static class TurboFacMetaRegisterExtension
	{
		static readonly Mining Mining = new StaticMining();

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

			var types = assemblies.SelectMany(x => x.GetTypes()).Where(x =>
				x.Attribute<RegisterServiceAttribute>() != null ||
				x.Attribute<DefaultImplAttribute>() != null );

			foreach (var type in types)
			{
				// writer.WriteLine("// " + type.FullName);
				var ifaces = Mining.GetAdditionalIfaces(type);
				foreach (var iface in ifaces)
				{
//					var ci = Mining.GetConstructor(type);
//					var arguments = string.Join(", ", Mining.GetConstructorArguments(ci));
//					// todo remove dependancy on 'c'
//					writer.WriteLine("c.Add<{0}>(()=>new {1}({2}));", iface.CSharpTypeIdentifier(), type.CSharpTypeIdentifier(), arguments);

					writer.WriteLine("c.Add<{0}>({1});", iface.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
				}
				var def = type.Attribute<DefaultImplAttribute>();
				if (type.IsInterface && def!=null)
				{
					// TODO
				}
				if (ifaces.Count() == 0 && type.Attribute<RegisterServiceAttribute>() != null)
				{
//					var ci = Mining.GetConstructor(type);
//					var arguments = string.Join(", ", Mining.GetConstructorArguments(ci));
//					// todo remove dependancy on 'c'
//					writer.WriteLine("c.Add(()=>new {1}({2}));", null, type.CSharpTypeIdentifier(), arguments);

					writer.WriteLine("c.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
				}
			}

			foreach (var type in types)
			{
				if (type.IsClass && !type.IsAbstract)
				{
					var plumbingPros = TurboContainer.GetPlumbingProperties(type).ToArray();
					if (plumbingPros.Length > 0)
					{
						var plumbing = "";
						foreach (var pi in plumbingPros)
						{
							plumbing += string.Format("item.{0} = c.Get<{1}>();\r\n", pi.Name, pi.PropertyType.CSharpTypeIdentifier());
						}
						// todo remove extra cast in plumber
						// todo remove dependancy on 'c'
						writer.WriteLine(@"TurboFac.Internal.TypeAutoPlumberRegister.Register<{0}>(x=>{{
var item = ({0})x;
{1}
				}});", type.CSharpTypeIdentifier(), plumbing);
					}
					else
					{
						writer.WriteLine("TurboFac.Internal.TypeAutoPlumberRegister.RegisterNone<{0}>();", type.CSharpTypeIdentifier());
					}
				}
			}
		}


	}
}
//}
