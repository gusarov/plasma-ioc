using TurboFac;

namespace TurboFacTests.Sample
{
	[DefaultImpl(typeof(MyService))]
	public interface IMyService
	{
		int MyMethod();
	}
}