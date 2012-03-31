using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

using TurboFac.Internal;

#if NET3
using MyUtils;
#endif


namespace TurboFac
{
	public sealed class TurboContainer : ITurboContainer
	{
		static ReflectionPermission _defaultReflectionPermission = ReflectionPermission.DebuggerBreakIfAttached;
		public static ReflectionPermission DefaultReflectionPermission
		{
			get { return _defaultReflectionPermission; }
			set { _defaultReflectionPermission = value; }
		}

		ReflectionPermission? _reflectionPermission;
		public ReflectionPermission ReflectionPermission
		{
			get { return _reflectionPermission ?? DefaultReflectionPermission; }
			set { _reflectionPermission = value; }
		}

		static readonly TurboContainer _root = new TurboContainer();

		public static TurboContainer Root
		{
			get { return _root; }
		}

		public TurboContainer()
		{
			__mining = new ReflectionMining(this);

			// current instance returns allways itself for interfaces
			this.Add<ITurboProvider>(this);
			this.Add<ITurboContainer>(this);

			//LoadAppConfig();
		}

		readonly Mining __mining;
		Mining Mining
		{
			get
			{
				// ValidateReflectionPermissions();
				return __mining;
			}
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
			readonly Lazy<object> _serviceGetLazy;
			readonly Lazy<object> _instance;
			readonly TurboContainer _provider;

			public ServiceEntry(TurboContainer provider, Lazy<object> instance)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				_instance = instance;
				_provider = provider;

				if (instance.IsValueCreated) // TODO if instance already created - no plumbing occurs!
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

			public Lazy<object> Instance
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
			// TODO infinite recursion by implementation ci
			void Plumb(object instance)
			{
				if (!TypeAutoPlumberRegister.TryPlumb(instance, _provider))
				{
					PlumbReflectionBased(instance);
				}
			}

			void PlumbReflectionBased(object instance)
			{
				_provider.ValidateReflectionPermissions();
				foreach (var pro in GetPlumbingProperties(instance.GetType()))
				{
					if (pro.GetValue(instance, null) == null)
					{
						pro.SetValue(instance, _provider.Mining.GetArgument(pro), null);
					}
				}
			}

		}

		internal static IEnumerable<PropertyInfo> GetPlumbingProperties(Type type)
		{
			foreach (var pro in type.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
				.Where(x =>
				       x.GetCustomAttributes(typeof(InjectAttribute), true).Any()
				       || x.GetCustomAttributes(typeof(DefaultImplAttribute), true).Any()
				)
				)
			{
				if (pro.GetIndexParameters().Length == 0 && pro.CanRead && pro.CanWrite)
				{
					if (pro.PropertyType.IsInterface || pro.PropertyType.IsAbstract || IsLazyOrFunc(pro.PropertyType))
					{
						// IEnumerable<T> and all generics
						if (pro.PropertyType.IsGenericType && !IsLazyOrFunc(pro.PropertyType))
						{
							continue;
						}
						// IEnumerable
						if (typeof(IEnumerable).IsAssignableFrom(pro.PropertyType))
						{
							continue;
						}
						yield return pro;
					}
				}
			}
		}

		static bool IsLazyOrFunc(Type propertyType)
		{
			return
				propertyType.IsGenericType &&
				(propertyType.GetGenericTypeDefinition() == typeof(Lazy<>) ||
				 propertyType.GetGenericTypeDefinition() == typeof(Func<>));
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
			ValidateReflectionPermissions();
			PerformAdd(type, new ServiceEntry(this, new Lazy<object>(() => Mining.DefaultFactory(implementation))));
		}

//		public void Add(Type type, Func<object> instanceFactory)
//		{
//			PerformAdd(type, ServiceEntry.Create(instanceFactory));
//		}

		public void Add(Type type, Lazy<object> instanceFactory)
		{
			PerformAdd(type, new ServiceEntry(this, instanceFactory));
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
			ValidateReflectionPermissions();
			PerformAdd(type, new ServiceEntry(this, (Lazy<object>)Mining.DefaultFactory(type)));
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

						var services = _services.Values.Select(x => x.InstanceCache)/*.Concat(new[] {_parentProvider})*/.Where(x => x != null).Distinct().OfType<IDisposable>().ToArray();

						var asyncs = services.Select(x => new Action<IDisposable>(y => y.Dispose()).BeginInvoke(x, null, null)).ToArray();
						foreach (var asyncResult in asyncs)
						{
							asyncResult.AsyncWaitHandle.WaitOne();
						}
					}
				}
			}
		}

//		static IAsyncResult DisposeAsync(IDisposable disposable)
//		{
//			if (disposable != null)
//			{
//				return new Action<IDisposable>(x => x.Dispose()).BeginInvoke(disposable, null, null);
//			}
//			return null;
//		}
//
//		static void WaitAll(IEnumerable<IAsyncResult> asyncResults)
//		{
//			if (asyncResults != null)
//			{
//				var handlesArray = asyncResults.Select(x => x.AsyncWaitHandle).ToArray();
//				foreach (var waitHandle in handlesArray)
//				{
//					waitHandle.WaitOne();
//				}
//			}
//		}

		#endregion


		/// <summary>
		/// Get from current container or from parent chain
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		internal Lazy<object> TryGetLazyCore(Type type)
		{
			Aspect();
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Lazy<>) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Func<>))
			{
				throw new TurboFacException("Do not request type by Lazy<T> or Func<T>. Specify requested type explicitly. Result is lazy anyway.");
			}
			ServiceEntry result;
			_services.TryGetValue(type, out result);
			if (result == null && _parentProvider != null)
			{
				return _parentProvider.TryGetLazy(type);
			}
			return result != null ? result.Instance : null;
		}

		internal Lazy<object> GetLazyCore(Type type)
		{
			return GetLazyCore(type, true);
		}

		internal Lazy<object> GetLazyCore(Type type, bool tryAutoReg)
		{
			Aspect();
			var result = TryGetLazyCore(type);

			if (result == null)
			{
				// try register in current container
				if (tryAutoReg && type.IsInterface)
				{
					//try {
						Add(type);
					//} catch (TurboFacException) {}
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

		/// <summary>
		/// reference type and not string
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		internal static bool IsValidImplementationType(Type type)
		{
			return !type.IsValueType && type != typeof(string);
		}

		internal static void ValidateImplementationType(Type type)
		{
			if (!IsValidImplementationType(type))
			{
				throw new TurboFacException("Can not register string or value type implementation");
			}
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
				var ifaces = Mining.GetAdditionalIfaces(type);
				foreach (var iface in ifaces)
				{
					_services[iface] = entry;
				}
			}
		}

		[DebuggerStepThrough]
		internal void ValidateReflectionPermissions()
		{
			const string message = "TurboContainer - Reflection based action";
			switch (ReflectionPermission)
			{
				case ReflectionPermission.Allow:
					break;
				case ReflectionPermission.Log:
					Trace.TraceError(message);
					break;
				case ReflectionPermission.DebuggerBreakIfAttached:
					if (Debugger.IsAttached)
					{
						Debugger.Break();
					}
					break;
				case ReflectionPermission.DebugAssertion:
					Debug.Fail(message);
					break;
				case ReflectionPermission.Throw:
					throw new TurboFacException(message);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
