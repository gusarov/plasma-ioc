using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

using Plasma.ThirdParty;

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
			return "default(" + type.CSharpTypeIdentifier(false) + ")";
		}

		protected override object DefaultFactoryCore(Type type)
		{
			var ci = GetConstructor(type);
			var arguments = string.Join(", ", GetConstructorArguments(ci));
			return string.Format("c => new {0}({1})", type.CSharpTypeIdentifier(false), arguments);
		}

		protected override object GetArgumentDefaultOptional(object defaultValue)
		{
			return _ignoreOptionalArgument;
		}

		protected override object GetArgumentRquestAndConvert(Type parameterType, Type requestedType, ICustomAttributeProvider info, bool isOptional)
		{
			var suggested = info.Attribute<DefaultImplAttribute>();
			var suggestedType = suggested == null ? null : suggested.TargetType;

			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
			{
				if (suggestedType != null)
					return string.Format("new Lazy<{0}>(c.{3}Get<{1}, {2}>)", Unlazy(parameterType).CSharpTypeIdentifier(false), requestedType.CSharpTypeIdentifier(), suggestedType.CSharpTypeIdentifier(), isOptional ? "Try" : "");
				return string.Format("new Lazy<{0}>(c.{2}Get<{1}>)", Unlazy(parameterType).CSharpTypeIdentifier(false), requestedType.CSharpTypeIdentifier(), isOptional ? "Try" : "");
			}
			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Func<>))
			{
				if (suggestedType != null)
					return string.Format("c.{2}Get<{0}, {1}>", requestedType.CSharpTypeIdentifier(false), suggestedType.CSharpTypeIdentifier(), isOptional ? "Try" : "");
				return string.Format("c.{1}Get<{0}>", requestedType.CSharpTypeIdentifier(false), isOptional ? "Try" : "");
			}
			if(suggestedType != null)
				return string.Format("c.{2}Get<{0}, {1}>()", requestedType.CSharpTypeIdentifier(false), suggestedType.CSharpTypeIdentifier(), isOptional ? "Try" : "");
			return string.Format("c.{1}Get<{0}>()", requestedType.CSharpTypeIdentifier(false), isOptional ? "Try" : "");
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