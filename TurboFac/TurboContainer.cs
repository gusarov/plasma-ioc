using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#if NET3
using MyUtils;
#endif


namespace TurboFac
{
	public sealed class TurboContainer : ITurboContainer
	{
		static readonly TurboContainer _root = new TurboContainer();

		public static TurboContainer Root
		{
			get { return _root; }
		}

		public TurboContainer()
		{
			// current instance returns allways itself for interfaces
			this.Add<ITurboProvider>(this);
			this.Add<ITurboContainer>(this);

			//LoadAppConfig();
		}

//		private void LoadAppConfig()
//		{
//			
//		}

		public TurboContainer(ITurboProvider parentProvider)
			: this()
		{
			if (this == parentProvider)
			{
				parentProvider = null;
			}
			_parentProvider = parentProvider;
		}

		readonly ITurboProvider _parentProvider;
		readonly Dictionary<Type, ServiceEntry> _services = new Dictionary<Type, ServiceEntry>();

		class ServiceEntry
		{
			readonly Lazy<object> _instance;
			readonly TurboContainer _provider;

			public ServiceEntry(Lazy<object> instance, TurboContainer provider)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				_instance = instance;
				_provider = provider;

				if (instance.IsValueCreated)
				{
					if (instance.Value != null)
					{
						ValidateImplementationType(instance.Value.GetType());
					}
				}

				_serviceGetLazy = new Lazy<object>(GetService);
			}

			/// <summary>
			/// Service instance or null if not instantiated yet
			/// </summary>
			public object InstanceCache
			{
				get { return _instance.IsValueCreated ? _instance.Value : null; }
			}

			readonly Lazy<object> _serviceGetLazy;

			public Lazy<object> Lazy
			{
				get { return _serviceGetLazy; }
			}

			object GetService()
			{
				if (!_instance.IsValueCreated)
				{
					lock (this)
					{
						if (!_instance.IsValueCreated)
						{
							var instance = _instance.Value;
							if (instance == null)
							{
								throw new TurboFacException("Instance factory returns null");
							}
							ValidateImplementationType(instance.GetType());
							Plumb(instance);
						}
					}
				}
				return _instance.Value;
			}

			// TODO extract to another class
			// TODO infinite recursion by implementation ctor
			void Plumb(object instance)
			{
				foreach (var pro in instance.GetType().GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
				{
					if (pro.GetIndexParameters().Length == 0 && pro.CanRead && pro.CanWrite)
					{
						if (pro.PropertyType.IsInterface || pro.PropertyType.IsAbstract || IsLazy(pro.PropertyType))
						{
							// IEnumerable<T> and all generics
							if (pro.PropertyType.IsGenericType && pro.PropertyType.GetGenericTypeDefinition()!=typeof(Lazy<>))
							{
								continue;
							}
							// IEnumerable
							if (typeof(IEnumerable).IsAssignableFrom(pro.PropertyType))
							{
								continue;
							}
							if (pro.GetValue(instance, null) == null)
							{
								pro.SetValue(instance, _provider.GetParameter(pro), null);
							}
						}
					}
				}
			}

			static bool IsLazy(Type propertyType)
			{
				return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Lazy<>);
			}
		}

		#region Interface

		public object Get(Type type)
		{
			return GetLazyCore(type).Value;
		}

		public Lazy<object> GetLazy(Type type)
		{
			return GetLazyCore(type);
		}

		public object TryGet(Type type)
		{
			var r = TryGetLazyCore(type);
			return r == null ? null : r.Value;
		}

		public Lazy<object> TryGetLazy(Type type)
		{
			return TryGetLazyCore(type);
		}

		public void Add(Type type, Type implementation)
		{
			PerformAdd(type, new ServiceEntry(DefaultFactory(implementation), this));
		}

//		public void Add(Type type, Func<object> instanceFactory)
//		{
//			PerformAdd(type, ServiceEntry.Create(instanceFactory));
//		}

		public void Add(Type type, Lazy<object> instanceFactory)
		{
			PerformAdd(type, new ServiceEntry(instanceFactory, this));
		}

//		public void Add<T>(Type type, Lazy<T> instanceFactory)
//		{
//			PerformAdd(type, ServiceEntry.Create(instanceFactory));
//		}
//
//		public void Add<T>(Type type, Func<T> instanceFactory)
//		{
//			PerformAdd(type, ServiceEntry.Create(instanceFactory));
//		}

		public void Add(Type type)
		{
			PerformAdd(type, new ServiceEntry(DefaultFactory(type), this));
		}

//		public void Add(Type type, object instance)
//		{
//			PerformAdd(type, ServiceEntry.Create(instance));
//		}

//		public void Add<T>(T instance)
//		{
//			PerformAdd(typeof(T), ServiceEntry.Create(instance));
//		}

//		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
//		public void Add<T>()
//		{
//			PerformAdd(typeof(T), ServiceEntry.Create(DefaultFactory(typeof(T))));
//		}
//
//		public void Add<T>(Func<T> instanceFactory)
//		{
//			PerformAdd(typeof(T), ServiceEntry.Create(instanceFactory));
//		}
//
//		public void Add<T, TImpl>(Lazy<TImpl> instanceFactory)
//		{
//			PerformAdd(typeof(T), ServiceEntry.Create(instanceFactory));
//		}
//
//		public void Add<T, TImpl>()
//		{
//			PerformAdd(typeof(T), ServiceEntry.Create(DefaultFactory(typeof(TImpl))));
//		}
//
//		public void Add<T>(Lazy<T> instanceFactory)
//		{
//			PerformAdd(typeof(T), ServiceEntry.Create(instanceFactory));
//		}

		bool _isDisposed;
		readonly object _disposeSync = new object();

		void Aspect()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				lock (_disposeSync)
				{
					if (!_isDisposed)
					{
						_isDisposed = true;

						var services = _services.Values.Select(x => x.InstanceCache).Concat(new[] {_parentProvider}).Where(x => x != null).Distinct().OfType<IDisposable>().ToArray();

						var asyncs = services.Select(x => new Action<IDisposable>(y => y.Dispose()).BeginInvoke(x, null, null)).ToArray();
						foreach (var asyncResult in asyncs)
						{
							asyncResult.AsyncWaitHandle.WaitOne();
						}
					}
				}
			}
		}

		public static IAsyncResult DisposeAsync(IDisposable disposable)
		{
			if (disposable != null)
			{
				return new Action<IDisposable>(x => x.Dispose()).BeginInvoke(disposable, null, null);
			}
			return null;
		}

		static void WaitAll(IEnumerable<IAsyncResult> asyncResults)
		{
			if (asyncResults != null)
			{
				var handlesArray = asyncResults.Select(x => x.AsyncWaitHandle).ToArray();
				foreach (var waitHandle in handlesArray)
				{
					waitHandle.WaitOne();
				}
			}
		}

		#endregion


		/// <summary>
		/// Get from current container or from parent chain
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		Lazy<object> TryGetLazyCore(Type type)
		{
			Aspect();
			ServiceEntry result;
			_services.TryGetValue(type, out result);
			if (result == null && _parentProvider != null)
			{
				return _parentProvider.TryGetLazy(type);
			}
			return result != null ? result.Lazy : null;
		}

		Lazy<object> GetLazyCore(Type type)
		{
			return GetLazyCore(type, true);
		}

		Lazy<object> GetLazyCore(Type type, bool tryAutoReg)
		{
			Aspect();
			var result = TryGetLazyCore(type);

			if (result == null)
			{
				// try register in current container
				if (tryAutoReg && type.IsInterface)
				{
					try
					{
						Add(type);
					}
					catch (TurboFacException) {}
					result = GetLazyCore(type, false);
				}
				//throw new TurboFacException("Service does not exists");
			}
			if (result == null)
			{
				throw new TurboFacException(string.Format(CultureInfo.CurrentCulture, "Service for '{0}' is not registered", type.Name));
			}
			return result;
		}

		Lazy<object> DefaultFactory(Type type)
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
					throw new TurboFacException(string.Format(CultureInfo.CurrentCulture, "Can not register service for type '{0}'. Specify instance, factory, or use DefaultImplAttribute", type.Name));
				}
			}
			ValidateImplementationType(type);

			return new Lazy<object>(() =>
			{
				ConstructorInfo ci;
				var args = GetConstructorArguments(type, out ci);
#if PocketPC
				return ci.Invoke(args);
#else
				return Activator.CreateInstance(type, args);
#endif
			});
		}

		/// <summary>
		/// reference type and not string
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		static bool IsValidImplementationType(Type type)
		{
			return !type.IsValueType && type != typeof(string);
		}

		static void ValidateImplementationType(Type type)
		{
			if (!IsValidImplementationType(type))
			{
				throw new TurboFacException("Can not register string or value type implementation");
			}
		}

		/// <summary>
		/// default(T) from type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		static object Default(Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

		void PerformAdd(Type type, ServiceEntry entry)
		{
			Aspect();
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(object))
			{
				throw new TurboFacException("Can not register service for System.Object. Use more specific type.");
			}
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}

			// if entry is created implicitly from object, it is already instantiated and we should perform additional calidations about a value for earlier error reporting
			if (entry.InstanceCache != null)
			{
				var instanceType = entry.InstanceCache.GetType();
				if (!type.IsAssignableFrom(instanceType))
				{
					throw new TurboFacException(string.Format(CultureInfo.CurrentCulture, "Service type {0} is not assignable from specified service instance of {1}", GetTypeName(type), GetTypeName(instanceType)));
				}
			}

			_services[type] = entry;

			RegisterTypesAutomatically(type, entry);
		}

		static string GetTypeName(Type type)
		{
			if (type.GetGenericArguments().Length > 0)
			{
				var main = type.Name.Substring(0, type.Name.IndexOf('`'));
				var inner = string.Join(",", type.GetGenericArguments().Select(x=>GetTypeName(x)).ToArray());
				return string.Format(CultureInfo.InvariantCulture, "{0}<{1}>", main, inner);
			}
			return type.Name;
		}

		void RegisterTypesAutomatically(Type type, ServiceEntry entry)
		{
			if (!type.IsInterface)
			{
				var ifaces = GetAdditionalIfaces(type);
				foreach (var iface in ifaces)
				{
					_services[iface] = entry;
				}
			}
		}

		object[] GetConstructorArguments(Type type, out ConstructorInfo ci)
		{
			var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			if (ctors.Length != 1)
			{
				if (ctors.Length == 0)
				{
					throw new TurboFacException(string.Format(CultureInfo.CurrentCulture, "No constructor for type '{0}'", type.Name));
				}

				ctors = ctors.Where(x => x.GetCustomAttributes(typeof(DefaultConstructorAttribute), false).Any()).ToArray();
				if (ctors.Length != 1)
				{
					throw new TurboFacException(string.Format(CultureInfo.CurrentCulture, "Ambiguous constructor for type '{0}'", type.Name));
				}
			}
			ci = ctors.Single();
			return ci.GetParameters().Select(x=>GetParameter(x)).ToArray();
		}

		object GetParameter(ParameterInfo parameter)
		{
			return GetParameter(parameter.ParameterType, parameter,
#if NET4
				parameter.IsOptional,
				parameter.RawDefaultValue
#else
				false,
				null
#endif
 );
		}

		object GetParameter(PropertyInfo property)
		{
			return GetParameter(property.PropertyType, property);
		}

		object GetParameter(Type parameterType, ICustomAttributeProvider attributes)
		{
			return GetParameter(parameterType, attributes, false);
		}

		object GetParameter(Type parameterType, ICustomAttributeProvider attributes, bool isOptional)
		{
			return GetParameter(parameterType, attributes, isOptional, null);
		}

		object GetParameter(Type parameterType, ICustomAttributeProvider attributes, bool isOptional, object defaultValue )
		{
			return GetParameter(parameterType, attributes.GetCustomAttributes(false).Cast<Attribute>(), isOptional, defaultValue);
		}

		object GetParameter(Type parameterType, IEnumerable<Attribute> attributes)
		{
			return GetParameter(parameterType, attributes, false);
		}

		object GetParameter(Type parameterType, IEnumerable<Attribute> attributes, bool isOptional)
		{
			return GetParameter(parameterType, attributes, isOptional, null);
		}

		object GetParameter(Type parameterType, IEnumerable<Attribute> attributes, bool isOptional, object defaultValue)
		{
			// validate
			if (!IsValidImplementationType(parameterType))
			{
				if (isOptional)
				{
					return defaultValue;
				}
				else
				{
					return Default(parameterType);
				}
			}

			// detect requestType
			var requestType = parameterType;

			if (parameterType.IsGenericType)
			{
				if (parameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
				{
					requestType = parameterType.GetGenericArguments()[0];
				}
			}

			// request

			var suggesstionAttribute = attributes.OfType<DefaultImplAttribute>().SingleOrDefault<DefaultImplAttribute>();
			var suggestedType = suggesstionAttribute == null ? null : suggesstionAttribute.TargetType;

			if (suggestedType != null)
			{
				var now = TryGetLazyCore(requestType);
				if (now == null)
				{
					Add(requestType, suggestedType);
				}
			}

			Lazy<object> serviceLazy;
			if (isOptional)
			{
				serviceLazy = TryGetLazyCore(requestType);
				if (serviceLazy == null)
				{
					return null;
				}
			}
			else
			{
				serviceLazy = GetLazyCore(requestType);
			}

			// convert to parameterType

			if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Lazy<>))
			{
				return new TypedLazyWrapper(parameterType.GetGenericArguments()[0], serviceLazy).Lazy;
			}

			if(parameterType.IsAssignableFrom(serviceLazy.GetType()))
			{
				return serviceLazy;
			}
			var value = serviceLazy.Value;
			if (parameterType.IsAssignableFrom(value.GetType()))
			{
				return value;
			}
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Can not provide value for {0}", parameterType));
		}

		static IEnumerable<Type> GetAdditionalIfaces(Type type)
		{
			if (!type.IsInterface)
			{
				var ifaces = type.GetInterfaces();
				if (ifaces.Count() == 1)
				{
					yield return ifaces.Single();
				}
			}
		}
	}
}
