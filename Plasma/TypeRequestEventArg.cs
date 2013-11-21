using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma
{
	internal class TypeRequestEventArg : EventArgs
	{
		private readonly Type _type;

		public Type Type
		{
			get { return _type; }
		}

		public TypeRequestEventArg(Type type)
		{
			_type = type;
		}

		public static implicit operator TypeRequestEventArg(Type type)
		{
			return new TypeRequestEventArg(type);
		}

		public static implicit operator Type(TypeRequestEventArg arg)
		{
			return arg.Type;
		}
	}
}