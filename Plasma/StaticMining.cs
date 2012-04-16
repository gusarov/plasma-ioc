using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

#if NET3
using MyUtils;
#endif

namespace Plasma
{
	class StaticMining : Mining
	{
		#region Done

		protected override object DefaultReferenceType(Type type)
		{
			return "null";
		}

		protected override object DefaultValueType(Type type)
		{
			return "default(" + type.CSharpTypeIdentifier() + ")";
		}

		protected override object DefaultFactoryCore(Type type)
		{
			var ci = GetConstructor(type);
			var arguments = string.Join(", ", GetConstructorArguments(ci));
			return string.Format("()=>new {0}({1})", type.CSharpTypeIdentifier(), arguments);
		}

		protected override object GetArgumentDefaultOptional(object defaultValue)
		{
			return _ignoreOptionalArgument;
		}

		protected override object GetArgumentRquestAndConvert(Type parameterType, Type requestedType, ICustomAttributeProvider info, bool isOptional)
		{
			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
			{
				return string.Format("new Lazy<{0}>(c.Get<{1}>)", Unlazy(parameterType).CSharpTypeIdentifier(), requestedType.CSharpTypeIdentifier());
			}
			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Func<>))
			{
				return string.Format("c.Get<{0}>", requestedType.CSharpTypeIdentifier());
			}
			return string.Format("c.Get<{0}>()", requestedType.CSharpTypeIdentifier());
		}

		public override object[] GetConstructorArguments(ConstructorInfo ci)
		{
			return ci.GetParameters().Select(GetArgument).Where(x => x != _ignoreOptionalArgument).ToArray();
		}

		static readonly object _ignoreOptionalArgument = new object();

		#endregion

//		public override object GetArgument(Type type, ICustomAttributeProvider info, bool optional, object defaultValue)
//		{
//			throw new NotImplementedException();
//		}

//		static object Getter(ParameterInfo parameterInfo)
//		{
//						if (parameterInfo.ParameterType.IsGenericType && parameterInfo.ParameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
//						{
//							return string.Format("new Lazy<{0}>(c.Get<{0}>)", Unlazy(parameterInfo.ParameterType).CSharpTypeIdentifier());
//						}
//						if (parameterInfo.ParameterType.IsGenericType && parameterInfo.ParameterType.GetGenericTypeDefinition() == typeof(Func<>))
//						{
//							return string.Format("c.Get<{0}>", Unlazy(parameterInfo.ParameterType).CSharpTypeIdentifier());
//						}
//						return string.Format("c.Get<{0}>()", parameterInfo.ParameterType.CSharpTypeIdentifier());
//		}


	}
}