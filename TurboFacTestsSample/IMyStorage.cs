using TurboFac;

namespace TurboFacTests.Sample
{
	[DefaultImpl(typeof(MyInmemStorage))]
	public interface IMyStorage
	{
	}

	public class MyInmemStorage : IMyStorage
	{
	}

	public class MyPipeStorage : IMyStorage
	{
	}

	public class MyFileStorage : IMyStorage
	{
	}

	public class MyNodeHost
	{
		[DefaultImpl(typeof(MyFileStorage))]
		public IMyStorage Storage { get; set; }
	}

	public class MyObjectMan
	{
		readonly IMyStorage _storage;


		public MyObjectMan([DefaultImpl(typeof(MyPipeStorage))] IMyStorage storage)
		{
			_storage = storage;
		}

		public IMyStorage Storage
		{
			get { return _storage; }
		}
	}
}
