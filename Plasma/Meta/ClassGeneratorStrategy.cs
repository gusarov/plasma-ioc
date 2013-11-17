using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

using MetaCreator;

namespace Plasma.Meta
{
	public abstract class ClassGeneratorStrategy
	{
		string _prefix = "";
		protected virtual string Prefix
		{
			get { return _prefix; }
		}

		string _postfix = "";
		protected virtual string Postfix
		{
			get { return _postfix; }
		}

		protected virtual string BaseClass { get { return string.Empty; } }

		protected virtual void BeforeMembers(){}

//		public string ShortNameFrom(Type type)
//		{
//			return ShortNameFrom(Prefix, type);
//		}
//
//		static string ShortNameFrom(string prefix, Type type)
//		{
//			var i = type.Name.IndexOf('`');
//			var name = type.Name;
//			if (i > 0)
//			{
//				name = name.Substring(0, i);
//			}
//			return prefix + (name.StartsWith("I") ? name.Substring(1) : type.Name);
//		}

		// context
		protected string _typeShortName;
		protected string _typeShortNameGeneric;
		protected string _typeFullNameGeneric;

		public class TypeName
		{
			public string Name { get; set; }
			public string NameGeneric { get; set; }
			public string FullNameGeneric { get; set; }
			public string FullNameGenericNoT { get; set; }
		}

		public TypeName GetTypeName(Type type)
		{
			return GetTypeName(Prefix, Postfix, type);
		}

		public static TypeName GetTypeName(string prefix, string postfix, Type type)
		{
			return new TypeName
			{
				Name = prefix + CutIfaceI(CutBackAp(type.Name)) + postfix,
				NameGeneric = prefix + CutIfaceI(CutNamespace(type.CSharpTypeIdentifier())) + postfix,
				FullNameGeneric = CombineNameSpace(LeaveNamespace(type.CSharpTypeIdentifier()), prefix + CutIfaceI(CutNamespace(type.CSharpTypeIdentifier()))) + postfix,
				FullNameGenericNoT = CombineNameSpace(LeaveNamespace(type.CSharpTypeIdentifier()), prefix + CutIfaceI(CutNamespace(type.CSharpTypeIdentifier(new SharpGenerator.TypeIdentifierConfig
				{
					UseNamedTypeParameters = false,
					UseEngineImports = true,
				})))) + postfix,
			};
		}

		// [Pure]
		static string CombineNameSpace(string a, string b)
		{
			if (string.IsNullOrEmpty(a))
			{
				return b;
			}
			if (string.IsNullOrEmpty(b))
			{
				return a;
			}
			return a + "." + b;
		}

		// [Pure]
		static string CutIfaceI(string name)
		{
			return name.StartsWith("I") ? name.Substring(1) : name;
		}

		// [Pure]
		static string CutBackAp(string name)
		{
			var i = name.IndexOf('`');
			if (i > 0)
			{
				name = name.Substring(0, i);
			}
			return name;
		}

		// [Pure]
		static string LeaveNamespace(string name)
		{
			var i = name.IndexOf('.');
			if (i > 0)
			{
				name = name.Substring(0, i);
			}
			return "";
		}

		// [Pure]
		static string CutNamespace(string name)
		{
			var i = name.LastIndexOf('.');
			if (i > 0)
			{
				name = name.Substring(i + 1);
			}
			return name;
		}

		protected IMetaWriter _writer;
		protected Type _type;

		public void Generate(IMetaWriter writer, Type type)
		{
			_writer = writer;
			_type = type;

			writer.WriteLine("// iface - " + type.Name);
			//			writer.WriteLine("// all ifaces - " + string.Join(", ", Mining.GetAllIfaces(type).Distinct().Select(x => x.Name)));

			var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance).Concat(Mining.GetAllIfaces(type).Distinct().SelectMany(x => x.GetMembers())).ToArray();

//			if (!members.Any())
//			{
//				return;
//			}

			var names = GetTypeName(type);
			_typeShortName = names.Name;
			_typeShortNameGeneric = names.NameGeneric;
			_typeFullNameGeneric = names.FullNameGeneric;

			writer.WriteLine("public class {0} : {2} {1}", _typeShortNameGeneric, type.CSharpTypeIdentifier(), string.Format(BaseClass, type.CSharpTypeIdentifier()));
			writer.WriteLine("{");

			BeforeMembers();

			var methods = members.Where(x => x.MemberType == MemberTypes.Method).Cast<MethodInfo>().Where(x => !x.IsSpecialName).ToArray();
			foreach (var method in methods)
			{
				if (methods.Count(x => x.Name == method.Name) > 1 /* && method.ReturnType == typeof(object) */) // todo improve by comparing signature, not just name
				{
					writer.Write("\t{0} {4}.{1}{2}({3})", method.ReturnType.CSharpTypeIdentifier(), method.Name, null, string.Join(", ", method.GetParameters().Select(x => x.CSharpTypeIdentifier() + " " + x.Name).ToArray()), method.DeclaringType.CSharpTypeIdentifier());
				}
				else
				{
					writer.Write("\tpublic virtual {0} {1}{2}({3})", method.ReturnType.CSharpTypeIdentifier(), method.Name, null, string.Join(", ", method.GetParameters().Select(x => x.CSharpTypeIdentifier() + " " + x.Name).ToArray()));
				}


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

			var pros = members.Where(x => x.MemberType == MemberTypes.Property).Cast<PropertyInfo>().ToArray();
			foreach (var pro in pros)
			{
				var isIndexer = pro.GetIndexParameters().Any();

				var indexerParameters = string.Join(", ", pro.GetIndexParameters().Select(x => x.ParameterType.CSharpTypeIdentifier() + " " + x.Name).ToArray());
				indexerParameters = indexerParameters.Any() ? " [" + indexerParameters + "]" : null;

				var indexerArguments = string.Join(", ", pro.GetIndexParameters().Select(x => x.Name).ToArray());
				indexerArguments = indexerArguments.Any() ? "[" + indexerArguments + "]" : null;

				var myProName = isIndexer ? "this" : pro.Name;
				var orProName = isIndexer ? "" : "." + pro.Name;

				if (pros.Count(x => x.Name == pro.Name) > 1/* && pro.PropertyType == typeof(object)*/) // todo improve by comparing signature, not just name. +not just object but all except several obvious cases
				{
					writer.Write("\t{0} {3}.{1}{2}", pro.PropertyType.CSharpTypeIdentifier(), myProName, indexerParameters, pro.DeclaringType.CSharpTypeIdentifier());
				}
				else
				{
					writer.Write("\tpublic virtual {0} {1}{2}", pro.PropertyType.CSharpTypeIdentifier(), myProName, indexerParameters);
				}

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
}