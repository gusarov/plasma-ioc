using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Plasma.Internal;
using Plasma.ThirdParty;

#if NET3
using MyUtils;
#endif

namespace Plasma
{
	static class AssemblyAnalyzeCache
	{
		static readonly Dictionary<Assembly, Dictionary<string, IEnumerable<Type>>> _map
			= new Dictionary<Assembly, Dictionary<string, IEnumerable<Type>>>();

		static readonly Dictionary<Type, HashSet<Type>> _mapImpls
			= new Dictionary<Type, HashSet<Type>>();

		static Dictionary<string, IEnumerable<Type>> GetForAsm(Assembly asm)
		{
			Dictionary<string, IEnumerable<Type>> result;
			if (!_map.TryGetValue(asm, out result))
			{
				result = asm
					.GetTypes()
					.Where(x => x.IsPublic && x.IsClass && !x.IsAbstract)
					.GroupBy(x => x.Name)
					.ToDictionary(x => x.Key, x => (IEnumerable<Type>)x);
			}
			return result;
		}

		static HashSet<Type> ImplsListFor(Type type)
		{
			HashSet<Type> list;
			if (!_mapImpls.TryGetValue(type, out list))
			{
				_mapImpls[type] = list = new HashSet<Type>();
			}
			return list;
		}

		static internal void AnalyzeImpls()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
/*
					if (type.Name == "GenericObjectFactory" && Debugger.IsAttached)
					{
						Debugger.Break();
					}
 */
					if (type.IsClass && !type.IsAbstract && type.IsPublic)
					{
						foreach (var iface in GetAllIfaces(type).Distinct())
						{
							ImplsListFor(iface).Add(type);
						}
					}
				}
			}
		}

		static IEnumerable<Type> GetAllIfaces(Type type)
		{
			// this
			foreach (var iface in type.GetInterfaces())
			{
				yield return iface;
				foreach (var subFace in GetAllIfaces(iface))
				{
					yield return subFace;
				}
			}
			// base
			if (type.BaseType != null)
			{
				foreach (var subFace in GetAllIfaces(type.BaseType))
				{
					yield return subFace;
				}
			}
		}

		public static IEnumerable<Type> SearchFor(string name)
		{
			foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				IEnumerable<Type> result;
				var map = GetForAsm(asm);
				if (map.TryGetValue(name, out result))
				{
					foreach (var type in result)
					{
						yield return type;
					}
				}
			}
		}

		public static HashSet<Type> ImplsOf(Type type)
		{
			HashSet<Type> result;
			_mapImpls.TryGetValue(type, out result);
			return result;
		}

		public static void LoadAll()
		{
			var current = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in current)
			{
				LoadAll(assembly);
			}
		}

		private static readonly HashSet<Assembly> _asms = new HashSet<Assembly>();

		public static void LoadAll(Assembly assembly)
		{
			if (_asms.Add(assembly))
			{
				foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
				{
					var asm = Assembly.Load(referencedAssembly);
					LoadAll(asm);
				}
			}
		}
	}

	abstract class Mining
	{
		internal event EventHandler<TypeRequestEventArg> RequestByType;

		protected virtual void OnRequestByType(Type e)
		{
			var handler = RequestByType;
			if (handler != null)
			{
				handler(this, e);
			}
		}

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
				if (ctorsDef.Length > 1)
				{
					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "Multiple default constructor attribute for type '{0}'", type.Name));
				}
				if(ctorsDef.Length == 1)
				{
					return ctorsDef[0];
				}
				// try to get the most detailed constructor
				// todo do the same in static generator!
				Trace.TraceError(string.Format("Plasma: Several constructors detected for type {0}. Please, specify default constructor. A first most detailed constructor is used", type.Name));
				var ctor = ctors.OrderBy(x => x.GetParameters().Length).First();
				return ctor;
				/*
				if (ctorsDef.Length != 1)
				{
					var parameterLess = ctors.SingleOrDefault(x => x.GetParameters().Length == 0);
					if (parameterLess != null)
					{
						return parameterLess;
					}
					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "No parameterless constructor for type '{0}'", type.Name));
				}
				*/
			}
			return ctors[0];
		}

		/// <summary>
		/// Instantiate specified type
		/// </summary>
		public object CreateType(Type type)
		{
			type = IfaceImpl(type);
			PlasmaContainer.ValidateImplementationType(type);
			return DefaultFactoryCore(type);
		}

		public Type IfaceImpl(Type type)
		{
			var impl = FaceImplRegister.Get(type);
			if (impl != null)
			{
				return impl;
			}

			if (!type.IsAbstract && type.IsClass)
			{
				return type;
			}

			ValidateReflectionPermission();
			if (type.IsInterface || type.IsAbstract)
			{
				var dsias = (DefaultImplAttribute[])type.GetCustomAttributes(typeof(DefaultImplAttribute), true);
				if (dsias.Any())
				{
					var dsia = dsias.FirstOrDefault(x => x.TargetType != null);
					if (dsia == null)
					{
						throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "Service for type '{0}' cannot be loaded by non-resolvable DefaultImpl: " + string.Join(", ", dsias.Select(x => x.TypeAqn)), type.Name));
					}
					type = dsia.TargetType;
				}
				else
				{
					Exception exinner = null;
					try
					{
						if (type.IsInterface && type.Name.StartsWith("I"))
						{
							var expectedName = type.Name.Substring(1);

							var matched = AssemblyAnalyzeCache.SearchFor(expectedName)
								.Where(x => type.IsAssignableFrom(x))
								.ToArray();

							if (matched.Length == 1)
							{
								return matched[0];
							}

							var impls = AssemblyAnalyzeCache.ImplsOf(type);
							if (impls != null && impls.Count == 1)
							{
								return impls.First();
							}
							else if (impls == null)
							{
								throw new Exception(string.Format("Impls of type '{0}' not registered", type));
							}
							else
							{
								throw new Exception(string.Format("Impls of type '{0}': {1}", type.CSharpTypeIdentifier(), string.Join(", ", impls.Select(x => x.CSharpTypeIdentifier()))));
							}
						}
					}
					catch (Exception ex)
					{
						exinner = ex;
					}

					throw new PlasmaException(string.Format(CultureInfo.CurrentCulture, "Cannot register service for type '{0}'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface", type.Name), exinner);
				}
			}
			return type;

		}

		protected abstract void ValidateReflectionPermission();

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
