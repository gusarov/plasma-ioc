
```
	public class MyService : IMyService
	{
		readonly IWorker _worker;

		public MyService(IWorker worker)
		{
			_worker = worker;
		}

		public int MyMethod()
		{
			return _worker.Do();
		}
	}
```