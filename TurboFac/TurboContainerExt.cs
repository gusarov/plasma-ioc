using System.ComponentModel;
using System;

using TurboFac;

#if NET3
using MyUtils;
#endif


//namespace TurboFac
//{
	public static class TurboContainerExt
	{
		/// <summary>
		/// Get a service of registered type. On failure: throw exception.
		/// </summary>
		/// <exception cref="TurboFacException">Service is not registered</exception>
		public static T Get<T>(this ITurboProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			return (T)provider.Get(typeof(T));
		}

		/// <summary>
		/// Try get service of registered type and return null on failure
		/// </summary>
		public static T TryGet<T>(this ITurboProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			return (T)provider.TryGet(typeof(T));
		}

		/// <summary>
		/// Try get service of registered type or register by factory
		/// </summary>
		public static T Get<T>(this ITurboContainer container, Func<T> factory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			var result = container.TryGet<T>();
			if (ReferenceEquals(null, result))
			{
				result = factory();
				container.Add(result);
			}
			return result;
		}

#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		public static void Add(this ITurboContainer container, Type type, object instance)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(type, CreateLazy(instance));
		}

#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		public static void Add(this ITurboContainer container, Type type, Func<object> instanceFactory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(type, CreateLazy(instanceFactory));
		}

#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		public static void Add<T>(this ITurboContainer container, Type type, Lazy<T> instanceFactory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(type, CreateLazy(instanceFactory));
		}

#if !PocketPC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
#endif
		public static void Add<T>(this ITurboContainer container, Type type, Func<T> instanceFactory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(type, CreateLazy(instanceFactory));
		}

		/// <summary>
		/// Register service with default lazy factory
		/// </summary>
		/// <typeparam name="T">Registering type, e.g. MyServiceImpl</typeparam>
		/// <param name="container"></param>
		public static void Add<T>(this ITurboContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(typeof(T));
		}

		/// <summary>
		/// Register service
		/// </summary>
		/// <typeparam name="T">Registering type, e.g. IMyService</typeparam>
		/// <param name="container"></param>
		/// <param name="instance">Service instance</param>
		public static void Add<T>(this ITurboContainer container, T instance)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(typeof(T), CreateLazy(instance));
		}

		/// <summary>
		/// Register service factory
		/// </summary>
		/// <typeparam name="T">Registering type, e.g. IMyService</typeparam>
		/// <param name="container"></param>
		/// <param name="instanceFactory">Service factory</param>
		public static void Add<T>(this ITurboContainer container, Func<T> instanceFactory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(typeof(T), CreateLazy(instanceFactory));
		}

		/// <summary>
		/// Register service lazy
		/// </summary>
		/// <typeparam name="T">Registering type, e.g. IMyService</typeparam>
		/// <param name="container"></param>
		/// <param name="instanceFactory">Service lazy instance</param>
		public static void Add<T>(this ITurboContainer container, Lazy<T> instanceFactory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(typeof(T), CreateLazy(instanceFactory));
		}

		/// <summary>
		/// Register service lazy
		/// </summary>
		/// <typeparam name="T">Registering type, e.g. IMyService</typeparam>
		/// <typeparam name="TImpl">Service implementation, e.g. MyServiceImpl</typeparam>
		/// <param name="container"></param>
		/// <param name="instanceFactory">Service lazy instance</param>
		public static void Add<T, TImpl>(this ITurboContainer container, Lazy<TImpl> instanceFactory)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(typeof(T), CreateLazy(instanceFactory));
		}

		/// <summary>
		/// Register service with default factory
		/// </summary>
		/// <typeparam name="T">Registering type, e.g. IMyService</typeparam>
		/// <typeparam name="TImpl">Service implementation, e.g. MyServiceImpl</typeparam>
		/// <param name="container"></param>
		public static void Add<T, TImpl>(this ITurboContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(typeof(T), typeof(TImpl));
		}

		static Lazy<object> CreateLazy(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			var lazy = new Lazy<object>(() => instance);
			// lazy.IsValueCreated should be true
			var call = lazy.Value;
			if (call == null)
			{
				throw new TurboFacException("instance is not specified");
			}
			return lazy;
		}

		static Lazy<object> CreateLazy<T>(Lazy<T> instance)
		{
			return new Lazy<object>(() => instance.Value, false); // thread unsafe
		}

		static Lazy<object> CreateLazy(Func<object> instance)
		{
			return new Lazy<object>(instance); // thread safe
		}

		static Lazy<object> CreateLazy<T>(Func<T> instance)
		{
			return new Lazy<object>(() => instance()); // thread safe
		}
	}
//}