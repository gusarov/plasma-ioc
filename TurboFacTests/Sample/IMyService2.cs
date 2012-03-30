namespace TurboFacTests.Sample
{
	interface IMyService2
	{
		void MyMethod2();
		IMyService SubService { get; }
	}
}