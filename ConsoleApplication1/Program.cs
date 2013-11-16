using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
	class Program
	{
		public static void Test(out int q)
		{
			q = 5;
		}
		static void Main()
		{
			var mi = typeof(Program).GetMethod("Test");
			var pi = mi.GetParameters()[0];
			Console.WriteLine(pi.ParameterType.IsByRef);
			// Console.WriteLine(pi.ParameterType.GetElementType().Name);
		}
	}
}
