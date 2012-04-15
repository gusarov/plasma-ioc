using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Plasma.ThirdParty;

namespace Plasma
{
	abstract class Mining
	{
		#region Done

		/// <summary>
		/// Return default instance (null or empty structure)
		/// </summary>
		public object Default(Type type)
		{
			if (type.IsValueType)
			{
				return DefaultValueType(type);
			}
			return DefaultReferenceType(type);
		}

		protected abstract object DefaultReferenceType(Type type);
		protected abstract object DefaultValueType(Type type);


		public object GetArgument(PropertyInfo property)
		{
			return GetArgument(property.PropertyType, property, false, null);
		}

		public object GetArgument(ParameterInfo parameter)
		{
			return GetArgument(parameter.ParameterType, parameter,
#if NET4
 parameter.IsOptional,
				parameter.RawDefaultValue
#else
				false,
				null
#endif
);
		}

		public IEnumerable<Type> GetAdditionalIfaces(Type type)
		{
			if (!type.IsInterface)
			{
				var ifaces = type.GetInterfaces();
				if (ifaces.Length == 1)
				{
					yield return ifaces[0];
				}
			}
		}

		public ConstructorInfo GetConstructor(Type type)
		{
			var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			if (ctors.Length != 1)
			{
				if (ctors.Length == 0)
				{
					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "No constructor for type '{0}'", type.Name));
				}

				ctors = ctors.Where(x => x.GetCustomAttributes(typeof(DefaultConstructorAttribute), false).Any()).ToArray();
				if (ctors.Length != 1)
				{
					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "Ambiguous constructor for type '{0}'", type.Name));
				}
			}
			return ctors.Single();
		}

		/// <summary>
		/// How instantiate specified type
		/// </summary>
		public object DefaultFactory(Type type)
		{
			if (type.IsInterface || type.IsAbstract)
			{
				var dsia = (DefaultImplAttribute)type.GetCustomAttributes(typeof(DefaultImplAttribute), true).FirstOrDefault();
				if (dsia != null)
				{
					type = dsia.TargetType;
				}
				else
				{
					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "Can not register service for type '{0}'. Specify instance, factory, or use DefaultImplAttribute", type.Name));
				}
			}
			PlasmaContainer.ValidateImplementationType(type);
			return DefaultFactoryCore(type);
		}

		protected abstract object DefaultFactoryCore(Type type);

		#endregion

		protected abstract object GetArgumentDefaultOptional(object defaultValue);

		public object GetArgument(Type parameterType, ICustomAttributeProvider info, bool isOptional, object defaultValue)
		{
			// validate
			if (!PlasmaContainer.IsValidImplementationType(parameterType))
			{
				if (isOptional)
				{
					return GetArgumentDefaultOptional(defaultValue);
				}
				return Default(parameterType);// todo not tested for static mining
			}

			// detect requestedType
			var requestType = Unlazy(parameterType);

			return GetArgumentRquestAndConvert(parameterType, requestType, info, isOptional);
		}

		internal static Type Unlazy(Type type)
		{
			if (type.IsGenericType)
			{
				var def = type.GetGenericTypeDefinition();
				if (def == typeof(Lazy<>) || def == typeof(Func<>))
				{
					return type.GetGenericArguments()[0];
				}
			}
			return type;
		}

		protected abstract object GetArgumentRquestAndConvert(Type parameterType, Type requestedType, ICustomAttributeProvider info, bool isOptional);
		public abstract object[] GetConstructorArguments(ConstructorInfo ci);
	}

	class ReflectionMining : Mining
	{
		public override object[] GetConstructorArguments(ConstructorInfo ci)
		{
			return ci.GetParameters().Select(GetArgument).ToArray();
		}

		#region Done

		public ReflectionMining(PlasmaContainer provider)
		{
			_provider = provider;
		}

		readonly PlasmaContainer _provider;

		protected override object DefaultReferenceType(Type type)
		{
			return null;
		}

		protected override object DefaultValueType(Type type)
		{
			return Activator.CreateInstance(type);
		}

		protected override object DefaultFactoryCore(Type type)
		{
			_provider.ValidateReflectionPermissions();
			var ci = GetConstructor(type);
			var args = GetConstructorArguments(ci);
#if PocketPC
			return ci.Invoke(args);
#else
			return Activator.CreateInstance(type, args);
#endif
		}

		protected override object GetArgumentDefaultOptional(object defaultValue)
		{
			return defaultValue;
		}

		#endregion

		protected override object GetArgumentRquestAndConvert(Type parameterType, Type requestedType, ICustomAttributeProvider info, bool isOptional)
		{
			// request
			var suggesstionAttribute = info.Attribute<DefaultImplAttribute>();
			var suggestedType = suggesstionAttribute == null ? null : suggesstionAttribute.TargetType;

			if (suggestedType != null)
			{
				// TODO allow get with suggestion using API. As a result code will be reusable with static mining. And it is just useful feature.
				// TODO same in static mining
				var now = _provider.TryGetLazyCore(requestedType);
				if (now == null)
				{
					_provider.Add(requestedType, suggestedType);
				}
			}

			Lazy<object> serviceLazy;
			if (isOptional)
			{
				// TODO same in static mining
				serviceLazy = _provider.TryGetLazyCore(requestedType);
				if (serviceLazy == null)
				{
					return null;
				}
			}
			else
			{
				serviceLazy = _provider.GetLazyCore(requestedType);
			}

			// convert to parameterType

			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
			{
				_provider.ValidateReflectionPermissions();
				return TypedLazyWrapper.Create(parameterType.GetGenericArguments()[0], serviceLazy);
			}

			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Func<>))
			{
				_provider.ValidateReflectionPermissions();
				return TypedFuncWrapper.Create(parameterType.GetGenericArguments()[0], serviceLazy);
			}

			if (parameterType.IsInstanceOfType(serviceLazy))
			{
				return serviceLazy;
			}
			var value = serviceLazy.Value;
			if (parameterType.IsInstanceOfType(value))
			{
				return value;
			}
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Can not provide value for {0}", parameterType));
		}
	}

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
