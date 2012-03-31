using TurboFac;

namespace TurboFacTests.Sample
{
#if NET4
	[DefaultImpl(typeof(MyServiceWithOptionalArguments))]
	public interface IMyServiceWithOptionalArguments
	{
		IMyStorage Storage { get; }
		IMyWorker Worker { get; }
	}

	[RegisterService]
	public class MyServiceWithOptionalArguments : IMyServiceWithOptionalArguments 
	{
		readonly IMyStorage _storage;
		readonly IMyWorker _worker;

		public MyServiceWithOptionalArguments(IMyStorage storage, IMyWorker worker = null)
		{
			_storage = storage;
			_worker = worker;
		}

		public IMyStorage Storage
		{
			get { return _storage; }
		}

		public IMyWorker Worker
		{
			get { return _worker; }
		}
	}
#endif
}