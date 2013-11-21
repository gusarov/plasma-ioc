using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plasma.Internal
{
	/// <summary>
	/// Register of default interface implementations
	/// </summary>
	public static class FaceImplRegister
	{
		static readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type>();

		/// <summary>
		/// Add mapping
		/// </summary>
		public static void Register<TIFace, TImpl>()
		{
			_map[typeof(TIFace)] = typeof(TImpl);
		}

		/// <summary>
		/// Add mapping
		/// </summary>
		internal static void Register(Type face, Type impl)
		{
			_map[face] = impl;
		}

		internal static IEnumerable<KeyValuePair<Type, Type>> Enumerate
		{
			get { return _map; }
		}

		internal static Type Get(Type iface)
		{
			Type impl;
			_map.TryGetValue(iface, out impl);
			return impl;
		}
	}
}
