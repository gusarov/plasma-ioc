using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Plasma.ThirdParty;

#if NET3
using MyUtils;
#endif

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

		static public IEnumerable<Type> GetAdditionalIfaces(Type type)
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

		static public IEnumerable<Type> GetAllIfaces(Type type)
		{
			var ifaces = type.GetInterfaces();
			foreach (var iface in ifaces)
			{
				yield return iface;
				foreach (var subFace in GetAllIfaces(iface))
				{
					yield return subFace;
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

				var ctorsDef = ctors.Where(x => x.GetCustomAttributes(typeof(DefaultConstructorAttribute), false).Any()).ToArray();
				if (ctorsDef.Length != 1)
				{
					var parameterLess = ctors.SingleOrDefault(x => x.GetParameters().Length == 0);
					if (parameterLess != null)
					{
						return parameterLess;
					}
					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "No parameterless constructor for type '{0}'", type.Name));
				}
			}
			return ctors[0];
		}

		public Type DefaultFactoryType(Type type)
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
			return type;
		}

		/// <summary>
		/// How instantiate specified type
		/// </summary>
		public object DefaultFactory(Type type)
		{
			type = DefaultFactoryType(type);
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
}
