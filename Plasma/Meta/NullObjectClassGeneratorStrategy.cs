using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Plasma.Meta
{
	class NullObjectClassGeneratorStrategy : ClassGeneratorStrategy
	{
		public HashSet<Type> NullObjects = new HashSet<Type>();
		public LinkedList<Type> RequestedObjects = new LinkedList<Type>();
		public HashSet<Type> ExplicitRequests = new HashSet<Type>();

		protected override string Prefix
		{
			get { return "Null"; }
		}

		protected override void BeforeMembers()
		{
			NullObjects.Add(_type);
			_writer.Write("\tpublic static readonly {0} Instance = new {0}();", _typeFullNameGeneric);
		}

		protected override void WriteMethodBody(MethodInfo method)
		{
			foreach (var pi in method.GetParameters())
			{
				if (pi.IsOut)
				{
					_writer.WriteLine("{0} = {1};", pi.Name ,GetNullObjectExpression(pi.ParameterType.GetElementType(), pi.Info()));
				}
			}

			var type = method.ReturnType;
			if (type == typeof(void))
			{
			}
			else
			{
				_writer.WriteLine("return {0};", GetNullObjectExpression(type, method.Info()));
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
//			try
//			{
			_writer.WriteLine("return {0};", GetNullObjectExpression(pro.PropertyType, pro.Info()));
//			}
//			catch (Exception ex)
//			{
//				_writer.WriteLine("throw new Exception({0})", ex.Message);
//			}
		}

		protected override void WritePropertySetter(PropertyInfo pro)
		{
		}

		protected override void WriteIndexerSetter(PropertyInfo pro, string indexerArguments)
		{
		}



		string GetNullObjectExpression(Type type, string @where)
		{
			if (type == typeof(object))
			{
				return "Null.Object<object>()";
			}
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
			//			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			//			{
			//				return string.Format("Enumerable.Empty<{0}>()", type.GetGenericArguments()[0].CSharpTypeIdentifier());
			//			}
			if (type.IsArray)
			{
				if (type.GetArrayRank() == 1)
				{
					return string.Format("({0}[])Enumerable.Empty<{0}>()", type.GetElementType().CSharpTypeIdentifier());
				}
				throw new NotSupportedException("NullObject proxy - Array rank 1 only supported");
			}

			if (type.IsGenericParameter)
			{
				return string.Format("Null.Object<{0}>()", type.CSharpTypeIdentifier());
			}

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Lazy<>))
			{
				return string.Format("new Lazy<{0}>(Null.Object<{0}>)", type.GetGenericArguments()[0].CSharpTypeIdentifier());
			}

			if (type.IsInterface)
			{
				var reqType = type;
				if (reqType.IsGenericType)
				{
					reqType = reqType.GetGenericTypeDefinition();
				}
				else
				{
					// ExplicitRequests.Add(type);
				}
				if (!NullObjects.Contains(reqType))
				{
					RequestedObjects.AddLast(reqType);
				}
				return string.Format("{0}.Instance", GetTypeName(type).FullNameGeneric);
			}
			throw new NotSupportedException(string.Format("NullObject proxy - Not supported return value type '{0}' for {1}", type.CSharpTypeIdentifier(), @where));
			// TODO nullable - decide
			// TODO properties - decide
		}
	}
}