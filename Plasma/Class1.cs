using System;
using System.Collections.Generic;

namespace TurboFac
{
	public static class Fac
	{
		static readonly IContainer _root = new FacContainer();

		public static IContainer Root { get { return _root; } }
	}

	public interface IContainer
	{
		object Get(Type type);
		T Get<T>();
	}

	class FacContainer : IContainer
	{
		readonly Dictionary<Type, object> _dic = new Dictionary<Type, object>();

		class Entry
		{
			public Entry(object instance)
			{
				_factory = new Lazy<object>(() => instance);
			}

			Lazy<object> _factory;
		}

		public object Get(Type type)
		{
			object inst;
			if (!_dic.TryGetValue(type, out inst))
			{
				_dic[type] = inst = FactoryRegister.Invoke()
			}
		}

		public T Get<T>()
		{
			return (T) Get(typeof(T));
		}
	}

//	static class SingletoneFacContainer<T>
//	{
//		public static T _singletone;
//
//		public static T Singletone
//		{
//			get { return _singletone ?? (_singletone = Init()); }
//		}
//
//		static T Init()
//		{
//			
//		}
//	}

	public static class FactoryRegister
	{
		static readonly Dictionary<Type, Func<object>> _register = new Dictionary<Type, Func<object>>();

		public static void Register<T>(Func<T> factory)
		{
			_register[typeof(T)] = () => (object) factory();
		}

		public static T Invoke<T>()
		{
			return (T)Invoke(typeof(T));
		}

		public static object Invoke(Type type)
		{
			return _register[type]();
		}
	}
}
