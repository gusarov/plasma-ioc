using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma.Aop
{
	/// <summary>
	/// Base class for attributive pointcuts for aspects
	/// </summary>
	public class CacheAdvise : IMethodIntercepAdvise
	{
		class Input : IEquatable<Input>
		{
			readonly object[] _args;

			Input(object[] args)
			{
				_args = args;
			}

			public static implicit operator Input(object[] args)
			{
				return new Input(args);
			}

			public static implicit operator object[](Input input)
			{
				return input._args;
			}

			public bool Equals(Input other)
			{
				if (other == null)
				{
					return false;
				}
				if (other._args.Length == _args.Length)
				{
					for (int i = 0; i < _args.Length; i++)
					{
						if (_args[i] != other._args[i])
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				return Equals(obj as Input);
			}

			public override int GetHashCode()
			{
				var hash = 0;
				for (var i = 0; i < _args.Length; i++)
				{
					hash *= 397;
					hash ^= _args[i].GetHashCode();
				}
				return hash;
			}
		}

		readonly Dictionary<Input, object> _cachedCalculations = new Dictionary<Input, object>();

		public object Invoke(Func<object[], object> body, object[] args)
		{
			Input input = args;
			object result;
			if (!_cachedCalculations.TryGetValue(input, out result))
			{
				_cachedCalculations[input] = result = body(args);
			}
			return result;
		}
	}
}