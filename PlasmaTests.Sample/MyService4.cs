using System.Linq;
using System.Collections.Generic;
using System;
using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyService4 : IMyService4
	{
		[Inject]
		public IMyPerformer Performer { get; set; }

		public bool MyMethod()
		{
			return Performer != null;
		}

		public MyEnum MyEnum { get; set; }
		public int MyInt { get; set; }
	}

	public struct MyStruct
	{
		
	}

	public enum MyEnum
	{
		a,b,c
	}
}