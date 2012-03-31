using TurboFac;

namespace TurboFacTests.Sample
{
	[TurboReg]
	public class MyWorker : IMyWorker
	{
		public int Test
		{
			get { return 777; }
		}
	}
}