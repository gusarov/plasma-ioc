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

			foreach (var type in types.Where(x => !x.IsAbstract && !x.IsInterface && !x.IsGenericType && x.IsPublic))
			{
				writer.WriteLine("Plasma.Internal.TypeFactoryRegister.Add<{0}>({1});", type.CSharpTypeIdentifier(), Mining.DefaultFactory(type));
			}

			foreach (var type in types.Where(x => x.IsInterface))
			{
				writer.WriteLine("Null.Register<{0}>(Null{1}.Instance);", type.CSharpTypeIdentifier(), ClassGeneratorStrategy.NameFrom(type));
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
				var def = type.Attribute<DefaultImplAttribute>();
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

			writer.WriteLine(@"
	}
}

");

			foreach (var type in types.Where(x => x.IsInterface))
			{
				_proxyGenerator.Generate(writer, type);
				//GenerateStub(writer, type);
				_nullGenerator.Generate(writer, type);
			}
		}

		static readonly ClassGeneratorStrategy _proxyGenerator = new ProxyClassGeneratorStrategy();
		static readonly ClassGeneratorStrategy _nullGenerator = new NullObjectClassGeneratorStrategy();
	}

	abstract class ClassGeneratorStrategy
	{
		protected abstract string Prefix { get; }
		protected virtual string BaseClass { get { return string.Empty; } }

		protected virtual void BeforeMembers(){}

		public static string NameFrom(Type type)
		{
			return type.Name.StartsWith("I") ? type.Name.Substring(1) : type.Name;
		}

		// context
		protected string _name;
		protected IMetaWriter _writer;
		protected Type _type;

		public void Generate(IMetaWriter writer, Type type)
		{
			_writer = writer;
			_type = type;

			//			writer.WriteLine("// iface - " + type.Name);
			//			writer.WriteLine("// all ifaces - " + string.Join(", ", Mining.GetAllIfaces(type).Distinct().Select(x => x.Name)));

			var members = type.GetMembers().Concat(Mining.GetAllIfaces(type).Distinct().SelectMany(x => x.GetMembers())).ToArray();

//			if (!members.Any())
//			{
//				return;
//			}

			var name = _name = NameFrom(type);

			writer.WriteLine("public class {2}{0} : {3} {1}", name, type.CSharpTypeIdentifier(), Prefix, string.Format(BaseClass, type.CSharpTypeIdentifier()));
			writer.WriteLine("{");

			BeforeMembers();

			foreach (var method in members.Where(x => x.MemberType == MemberTypes.Method).Cast<MethodInfo>().Where(x => !x.IsSpecialName))
			{
				writer.Write("\tpublic virtual {0} {1}{2}({3})", method.ReturnType.CSharpTypeIdentifier(), method.Name, null, string.Join(", ", method.GetParameters().Select(x => x.ParameterType.CSharpTypeIdentifier() + " " + x.Name)));
				writer.Write(" { ");
				WriteMethodBody(method);
				writer.WriteLine(" }");
			}

			foreach (var eventInfo in members.Where(x => x.MemberType == MemberTypes.Event).Cast<EventInfo>())
			{
				writer.Write("\tpublic virtual event {0} {1}", eventInfo.EventHandlerType.CSharpTypeIdentifier(), eventInfo.Name);
				writer.Write(" { ");
				writer.Write(" add { ");
				WriteEventSubscriber(eventInfo);
				writer.Write(" }");
				writer.Write(" remove { ");
				WriteEventUnsubscriber(eventInfo);
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
				var orProName = isIndexer ? "" : "." + pro.Name;

				writer.Write("\tpublic virtual {0} {1}{2}", pro.PropertyType.CSharpTypeIdentifier(), myProName, indexerParameters);
				writer.Write(" { ");
				if (pro.CanRead)
				{
					writer.Write(" get { ");
					if (isIndexer)
					{
						WriteIndexerGetter(pro, indexerArguments);
					}
					else
					{
						WritePropertyGetter(pro);
					}
					writer.Write(" }");
				}
				if (pro.CanWrite)
				{
					writer.Write(" set { ");
					if (isIndexer)
					{
						WriteIndexerSetter(pro, indexerArguments);
					}
					else
					{
						WritePropertySetter(pro);
					}
					writer.Write(" }");
				}
				writer.WriteLine(" }");
			}

			writer.WriteLine("}");
		}

		protected abstract void WriteIndexerSetter(PropertyInfo pro, string indexerArguments);
		protected abstract void WritePropertySetter(PropertyInfo pro);
		protected abstract void WritePropertyGetter(PropertyInfo pro);
		protected abstract void WriteIndexerGetter(PropertyInfo pro, string indexerArguments);
		protected abstract void WriteEventUnsubscriber(EventInfo eventInfo);
		protected abstract void WriteEventSubscriber(EventInfo eventInfo);
		protected abstract void WriteMethodBody(MethodInfo method);
	}

	class ProxyClassGeneratorStrategy : ClassGeneratorStrategy
	{
		protected override string Prefix
		{
			get { return "Proxy"; }
		}

		protected override string BaseClass
		{
			get { return "Plasma.Proxy.ProxyBase<{0}>, "; }
		}

		protected override void BeforeMembers()
		{
			_writer.Write("\tpublic {2}{0}({1} originalObject) : base(originalObject)", _name, _type.CSharpTypeIdentifier(), Prefix);
			_writer.Write("\t{");
			_writer.WriteLine("\t}");
		}

		protected override void WriteMethodBody(MethodInfo method)
		{
			_writer.Write("{0}Original.{1}({2});", method.ReturnType == typeof(void) ? "" : "return ", method.Name, string.Join(", ", method.GetParameters().Select(x => x.Name)));
		}
		protected override void WriteEventSubscriber(EventInfo eventInfo)
		{
			_writer.Write("Original.{0} += value;", eventInfo.Name);
		}
		protected override void WriteEventUnsubscriber(EventInfo eventInfo)
		{
			_writer.Write("Original.{0} -= value;", eventInfo.Name);
		}
		protected override void WritePropertyGetter(PropertyInfo pro)
		{
			_writer.Write("return Original.{0};", pro.Name);
		}
		protected override void WriteIndexerGetter(PropertyInfo pro, string indexerArguments)
		{
			_writer.Write("return Original{0};", indexerArguments);
		}
		protected override void WritePropertySetter(PropertyInfo pro)
		{
			_writer.Write("Original.{0} = value;", pro.Name);
		}
		protected override void WriteIndexerSetter(PropertyInfo pro, string indexerArguments)
		{
			_writer.Write("Original{0} = value;", indexerArguments);
		}
	}

	class NullObjectClassGeneratorStrategy : ClassGeneratorStrategy
	{
		protected override string Prefix
		{
			get { return "Null"; }
		}

		protected override void BeforeMembers()
		{
			_writer.Write("\tpublic static readonly {2}{0} Instance = new {2}{0}();", _name, _type.CSharpTypeIdentifier(), Prefix);
		}

		protected override void WriteMethodBody(MethodInfo method)
		{
			var type = method.ReturnType;
			if (type == typeof(void))
			{
			}
			else
			{
				_writer.WriteLine("return {0};", GetNullObjectExpression(type));
			}
		}

		protected override void WriteEventSubscriber(EventInfo eventInfo)
		{
		}

		protected override void WriteEventUnsubscriber(EventInfo eventInfo)
		{
		}

		protected override void WriteIndexerGetter(PropertyInfo pro, string indexerArguments)
		{
			WritePropertyGetter(pro);
		}

		protected override void WritePropertyGetter(PropertyInfo pro)
		{
			try
			{
				_writer.WriteLine("return {0};", GetNullObjectExpression(pro.PropertyType));
			}
			catch (Exception ex)
			{
				_writer.WriteLine("throw new Exception({0})", ex.Message);
			}
		}

		protected override void WritePropertySetter(PropertyInfo pro)
		{
		}

		protected override void WriteIndexerSetter(PropertyInfo pro, string indexerArguments)
		{
		}

		string GetNullObjectExpression(Type type)
		{
			if (type == typeof(string))
			{
				return "string.Empty";
			}
			if (type.IsValueType)
			{
				return string.Format("default({0})", type.CSharpTypeIdentifier());
			}
			if (typeof(Delegate).IsAssignableFrom(type))
			{
				return "delegate { }";
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				return string.Format("Enumerable.Empty<{0}>()", type.GetGenericArguments()[0].CSharpTypeIdentifier());
			}
			if (!type.IsGenericType)
			{
				return string.Format("Null{0}.Instance", NameFrom(type));
			}

			throw new NotSupportedException("Not supported return value type: " + type.Name);
			// TODO nullable - decide
			// TODO properties - decide
		}
	}
}

