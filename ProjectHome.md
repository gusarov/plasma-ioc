The main challenge for this project is to outdo mature IoC containers. Primary goals are performance and usability, while providing most of the common features, including AOP. To facilitate this task I'm using my static meta-programming framework [MetaCreator](http://code.google.com/p/metacreator/) to generate Proxy Objects and any kind of reflection or IL weaving replacement.

In the nearest future I'll start to cover all Spring Framework features that I'm using in a really huge project to replace Spring (AOP, Spring.Data.Hibernate, attributed transactions, interceptors).

To download this project use `NuGet`: Plasma

There is no attributes required and no XML by default. Just express your dependencies in constructor. You can use Lazy or Func, you can use interface or class directly.

```
	public class CurrentStateManager
	{
		private readonly Lazy<IStorage> _storage;

		public CurrentStateManager(ISwitchEventProvider events, Lazy<IStorage> storage)
		{
			_storage = storage;
			events.Switch += EventsSwitch;
		}

		private void EventsSwitch(object sender, PeriodSwitchEventArgs e)
		{
			
		}
	}
```

Finally, when you need to buildup your app, you can use static root container or create your own, and it will work.

```
	public class Program
	{
		public static void Main()
		{
			var mgr1 = PlasmaContainer.Root.Get<CurrentStateManager>();
			// or
			var container = new PlasmaContainer();
			var mgr2 = container.Get<CurrentStateManagеr>();
		}
	}
```
