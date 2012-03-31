using System;

using TurboFac;

namespace TurboFacTests.Sample
{
	[TurboReg]
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