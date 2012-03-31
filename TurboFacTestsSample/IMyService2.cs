namespace TurboFacTests.Sample
{
	public interface IMyService2
	{
		void MyMethod2();
		IMyService SubService { get; }
	}
}