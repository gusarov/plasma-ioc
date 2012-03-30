using TurboFac;

namespace TurboFacTests.Sample
{
	[DefaultImpl(typeof(MyWorker))]
	public interface IMyWorker
	{
		int Test { get; }
	}
}