using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plasma.Meta
{
	static class Ext
	{
		public static string Info(this ParameterInfo pi)
		{
			return string.Format("parameter '{0}' of {1}", pi.Name, Info(pi.Member));
		}

		public static string Info(this MemberInfo mi)
		{
			return string.Format("member '{0}' in '{1}'", mi.Name, mi.DeclaringType.CSharpTypeIdentifier());
		}

		public static string CSharpTypeIdentifier(this ParameterInfo pi)
		{
			string prefix = null;
			string type = pi.ParameterType.CSharpTypeIdentifier();
			if (pi.ParameterType.IsByRef)
			{
				type = pi.ParameterType.GetElementType().CSharpTypeIdentifier();
				if (pi.IsOut)
				{
					prefix = "out ";
				}
				else
				{
					prefix = "ref ";
				}
			}
			return prefix + type;
		}
	}
}
