using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

using Plasma.Internal;
using Plasma.ThirdParty;

#if NET3
using MyUtils;
#endif

namespace Plasma
{
	class ReflectionMining : Mining
	{
		public override object[] GetConstructorArguments(ConstructorInfo ci)
		{
			return ci.GetParameters().Select(x => GetArgument(x)).ToArray();
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
			var suggesstionAttribute = info.Attribute2<DefaultImplAttribute>();
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
}