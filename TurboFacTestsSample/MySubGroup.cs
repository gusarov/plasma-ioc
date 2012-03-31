using TurboFac;

namespace TurboFacTests.Sample
{
	[RegisterService]
	public class MySubGroup
	{
		readonly ITurboProvider _provider;

		public MySubGroup(ITurboProvider provider)
		{
			_provider = provider;
		}

		public IMyWorker Worker { get { return _provider.Get<IMyWorker>(); } }
		public IMyPerformer Performer { get { return _provider.Get<IMyPerformer>(); } }
	}
}
