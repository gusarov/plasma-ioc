using TurboFac;

namespace TurboFacTests.Sample
{
	[RegisterService]
	public class MyService : IMyService
	{
		public static int Instantiated;

		public MyService()
		{
			Instantiated++;
		}

		public int MyMethod()
		{
			return Worker.Test;
		}

		[Inject]
		public IMyWorker Worker { get; set; }


	}
}