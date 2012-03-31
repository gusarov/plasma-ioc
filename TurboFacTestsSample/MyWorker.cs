using TurboFac;

namespace TurboFacTests.Sample
{
	[RegisterService]
	public class MyWorker : IMyWorker
	{
		public int Test
		{
			get { return 777; }
		}
	}
}