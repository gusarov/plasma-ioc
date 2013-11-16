using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Linq;
using Plasma;

#if NET3
using MyUtils;
#endif

#if !PocketPC
using Plasma.Meta;
using MetaCreator;
#endif

#if !PocketPC
public static class PlasmaMetaRegisterExtensionRoot
{
    public static void RegisterAll(this IMetaWriter writer)
    {
        PlasmaMetaRegisterExtension.PlasmaRegisterAll(writer);
    }
}
#endif

public static class PlasmaContainerExt
{
    /// <summary>
    /// Get a service of registered type. On failure: throw exception.
    /// </summary>
    /// <exception cref="PlasmaException">Service is not registered</exception>
    public static T Get<T>(this IPlasmaProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException("provider");
        }
        return (T)provider.Get(typeof(T));
    }

    public static T Get<T, TSuggest>(this IPlasmaProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException("provider");
        }
        return (T)provider.Get(typeof(T), typeof(TSuggest));
    }

    /// <summary>
    /// Try get service of registered type and return null on failure
    /// </summary>
    public static T TryGet<T>(this IPlasmaProvider provider)
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
    public static T Get<T>(this IPlasmaContainer container, Func<T> factory)
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
    public static void Add(this IPlasmaContainer container, Type type, object instance)
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
    public static void Add(this IPlasmaContainer container, Type type, Func<object> instanceFactory)
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
    public static void Add<T>(this IPlasmaContainer container, Type type, Lazy<T> instanceFactory)
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
    public static void Add<T>(this IPlasmaContainer container, Type type, Func<T> instanceFactory)
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
    /// <typeparam _typeShortName="T">Registering type, e.g. MyServiceImpl</typeparam>
    /// <param _typeShortName="container"></param>
    public static void Add<T>(this IPlasmaContainer container)
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
    /// <typeparam _typeShortName="T">Registering type, e.g. IMyService</typeparam>
    /// <param _typeShortName="container"></param>
    /// <param _typeShortName="instance">Service instance</param>
    public static void Add<T>(this IPlasmaContainer container, T instance)
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
    /// <typeparam _typeShortName="T">Registering type, e.g. IMyService</typeparam>
    /// <param _typeShortName="container"></param>
    /// <param _typeShortName="instanceFactory">Service factory</param>
    public static void Add<T>(this IPlasmaContainer container, Func<T> instanceFactory)
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
    /// <typeparam _typeShortName="T">Registering type, e.g. IMyService</typeparam>
    /// <param _typeShortName="container"></param>
    /// <param _typeShortName="instanceFactory">Service lazy instance</param>
    public static void Add<T>(this IPlasmaContainer container, Lazy<T> instanceFactory)
    {
        if (container == null)
        {
            throw new ArgumentNullException("container");
        }
        // TODO Eliminate lazy double wrapping: Lazy<T>(Lazy<object>(Lazy<T>()))
        container.Add(typeof(T), CreateLazy(instanceFactory));
    }

    /// <summary>
    /// Register service lazy
    /// </summary>
    /// <typeparam _typeShortName="T">Registering type, e.g. IMyService</typeparam>
    /// <typeparam _typeShortName="TImpl">Service implementation, e.g. MyServiceImpl</typeparam>
    /// <param _typeShortName="container"></param>
    /// <param _typeShortName="instanceFactory">Service lazy instance</param>
    public static void Add<T, TImpl>(this IPlasmaContainer container, Lazy<TImpl> instanceFactory)
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
    /// <typeparam _typeShortName="T">Registering type, e.g. IMyService</typeparam>
    /// <typeparam _typeShortName="TImpl">Service implementation, e.g. MyServiceImpl</typeparam>
    /// <param _typeShortName="container"></param>
    public static void Add<T, TImpl>(this IPlasmaContainer container)
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
            throw new PlasmaException("instance is not specified");
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

/// <summary>
/// Suggest this implementation type or assembly qualified type _typeShortName.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class DefaultImplAttribute : Attribute
{
    Type _type;
    readonly string _typeAqn;

    public DefaultImplAttribute(Type type)
    {
        _type = type;
    }

    public DefaultImplAttribute(string assemblyQualifiedName)
    {
        _typeAqn = assemblyQualifiedName;
    }

    public Type TargetType
    {
        get { return _type ?? (_type = Type.GetType(TypeAqn, false)); }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public string TypeAqn
    {
        get { return _typeAqn; }
    }
}

/// <summary>
/// Allow property injection here. Automatic property injection is not safe, consider explicit specification
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class InjectAttribute : Attribute
{
}

public static class Null
{
	static Null()
	{
		Register(new object());
		Register(string.Empty);
		RegisterGeneric(typeof (IEnumerable<>), t =>
			typeof (Enumerable).GetMethod("Empty").MakeGenericMethod(t).Invoke(null, null));
	}

	static readonly Dictionary<Type, object> _dic = new Dictionary<Type, object>();
	static readonly Dictionary<Type, Func<Type[], object>> _dicGeneric = new Dictionary<Type, Func<Type[], object>>();
	static readonly HashSet<Type> _accessed = new HashSet<Type>();

	public static void Register<T>(T instance) where T : class
	{
		Register(typeof(T), instance);
	}

	public static void Register(Type type, object instance)
	{
		object inst;
		if (_dic.TryGetValue(type, out inst))
		{
			if (_accessed.Contains(type) && !ReferenceEquals(inst, instance))
			{
				throw new PlasmaException("Can not register null object for type that already was registered and accessed");
			}
		}
		_dic[type] = instance;
	}

	public static T Object<T>()
	{
		return (T)Object(typeof(T));
	}

	public static object Object(Type type)
	{
		object value;
		if (_dic.TryGetValue(type, out value))
		{
			_accessed.Add(type);
			return value;
		}
		if (type.IsGenericType)
		{
			var gtd = type.GetGenericTypeDefinition();
			Func<Type[], object> fact;
			if (_dicGeneric.TryGetValue(gtd, out fact))
			{
				_accessed.Add(gtd);
				value = fact(type.GetGenericArguments());
				Register(type, value);
				return value;
			}
		}
		throw new PlasmaException("Null object is not registered for: " + type.CSharpTypeIdentifier());
	}

	public static void RegisterGeneric(Type typeDefinition, Func<Type[], object> factory)
	{
		Func<Type[], object> fact;
		if (_dicGeneric.TryGetValue(typeDefinition, out fact))
		{
			if (_accessed.Contains(typeDefinition) && !ReferenceEquals(fact, factory))
			{
				throw new PlasmaException("Can not register null object generic factory for generic type definition that already was registered and accessed");
			}
		}
		_dicGeneric[typeDefinition] = factory;
	}
}