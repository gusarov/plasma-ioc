# Introduction #

No, it is not a new required naming convention or something like this... Actually we naturally dislike any forced conventions. But we have a lot of them, and most noticeable one is 'I' for interfaces: `IMyService` and `MyService` clearly distinguish interface from class.

# Details #

So, we actually already have a bunch of conventions and an idea behind this framework is to not force you to use a new one or not to force you to use attributes, xmls, fluent code to configure container... Imagine you have next assemblies

```
MyProject.Contracts.dll
    IMyService
MyProject.Services.dll
    MyService
```

Actually any developer can clearly understand that MyService implements [IMyService](IMyService.md)... and that it is the only implementation in our solution. So, why you ever need any configuration for that? This convention is already in our mind! Someone prefer `MyServiceImpl` instead, and this is convention too. The only problem with such conventions - it is possible excessive usage leading to the fact that it is applied wider then required. That's why there is such a restriction like same first part of namespace, e.t.c.

# Reference #

So, here is conventions we use really often that are implemented in Plasma to understand what you want:

## Interface implementations: ##
  * Interface have the only one implementation in app domain (and first part of namespace is the same)
  * Interface name is equals to 'I' + implementation class name (and first part of namespace is the same)
  * Attribute: interface have DefaultImplAttribute with type or assembly qualified name specified
  * Programmatic: in even more difficult case you can configure container with types, like `PlasmaContainer.Root.Add<IMyService, TheServiceImpl>()`

## Register service in container: ##
  * in last version you are not forced to register it at all. Now you can request a service by type or by interface to provide better flexibility in scenarios. (Previous versions had a lot of restrictions, like, register and request only by interface. I'll add this restrictions optionally later to provide an ability to enforce patterns)
  * ~~`[RegisterService]` now becoming obsolete and had been used only for static mode~~

## Request a dependency: ##
  * Constructor Injection: Primary approach is to add an argument to constructor. You can also use Lazy or Func to postpone service creation as late as possible.
```
	public class MyService
	{
		private readonly Lazy<MyServiceDependency> _service;

		public MyService(Lazy<MyServiceDependency> service)
		{
			_service = service;
		}
	}
```
  * Property Injection: This is not recommended way because it technically provides an ability to leave a service partially configured and not forcing anybody to inject a dependency. But you can express such dependency in several ways (Lazy and Func are also supported here):
    * Attributed. To avoid disambiguation you have to use `[Inject]` attribute explicitly
    * Convention: leave getter private! This is also quite common convention to leave getter private for property injection since it prevents other entities to accidentally access your injections. Other entities have to request their dependencies by them selves. This convention clearly and quite safely distinguish property injection form usual property. But it breaks some minor code analysis rule about property without getter.
```
	public class MyService : IMyService
	{
		[Inject]
		public IMyWorker Worker1 { get; set; }

		public IMyWorker Worker2 { private get; set; }
	}
```