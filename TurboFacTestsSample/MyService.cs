using TurboFac;

namespace TurboFacTests.Sample
{
	[TurboReg]
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

		public IMyWorker Worker { get; set; }


	}
}