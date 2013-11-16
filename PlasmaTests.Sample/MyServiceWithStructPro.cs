using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyServiceWithStructPro
	{
		public MyServiceWithStructPro()
		{
			Test = "asd";
		}

		public string Test { get; set; }
		public Guid Test2 { get; set; }
	}
}