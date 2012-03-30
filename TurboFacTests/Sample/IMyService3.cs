using TurboFac;

namespace TurboFacTests.Sample
{
	[DefaultImpl(typeof(MyService3))]
	interface IMyService3
	{
		bool MyMethod();
	}
}