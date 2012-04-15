using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MyService3 : IMyService3
	{
		readonly IMyPerformer _performer;

		public MyService3(IMyPerformer performer)
		{
			if (performer == null)
			{
				throw new ArgumentNullException("performer");
			}
			_performer = performer;
		}

		public bool MyMethod()
		{
			return _performer != null;
		}
	}

	[RegisterService]
	public class MyService4 : IMyService4
	{
		[Inject]
		public IMyPerformer Performer { get; set; }

		public bool MyMethod()
		{
			return Performer != null;
		}
	}
}