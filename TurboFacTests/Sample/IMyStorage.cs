using TurboFac;

namespace TurboFacTests.Sample
{
	[DefaultImpl(typeof(MyInmemStorage))]
	interface IMyStorage
	{
	}

	class MyInmemStorage : IMyStorage
	{
	}

	class MyPipeStorage : IMyStorage
	{
	}

	class MyFileStorage : IMyStorage
	{
	}

	class MyNodeHost
	{
		[DefaultImpl(typeof(MyFileStorage))]
		public IMyStorage Storage { get; set; }
	}

	class MyObjectMan
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
