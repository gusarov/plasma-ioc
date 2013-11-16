using System.Linq;
using System.Collections.Generic;
using System;

using Plasma;

namespace PlasmaTests.Sample
{
	[RegisterService]
	public class MySubGroup
	{
		readonly IPlasmaProvider _provider;

		public MySubGroup(IPlasmaProvider provider)
		{
			_provider = provider;
		}

		public IMyWorker Worker { get { return _provider.Get<IMyWorker>(); } }
		public IMyPerformer Performer { get { return _provider.Get<IMyPerformer>(); } }
	}
}
