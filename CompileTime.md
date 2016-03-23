# Introduction #

Plasma can work as IoC and factory in reflection-based mode, but other features like NullObject, [Proxies](Proxies.md), [AOP](AOP.md) requires compile time injection.

The main motivation for that is performance. While other frameworks takes a lot of time (especially start-up time) to generate proxy in runtime and use a lot of reflection, Plasma in 'static mode' uses reflection only during compilation.

# How to enable #

The main thing that you have to understand is a compilation sequence and dependencies. You can not generate code in the same library as your services. Because my generator uses a library as input, not a source code. So, if you have `MyProject.Services` library, than most probably you have to add one more library like `MyProject.Services.Generated` that references `MyProject.Services` and add both Plasma IoC and MetaCreator projects from NuGet. Just before compilation of `MyProject.Services.Generated` your services library will be ready and compiled and in exact this moment Plasma will analyze it and generate necessary code which will be passed to compilation within `MyProject.Services.Generated` library.

To do that use Plasma's MetaCreator extensions:
```
namespace MyProject.Services.Generated
{
	// this helps you to locate generated code and provide an ability to include it in C# project which also facilitates ReSharper problem.
	/*@ fileInProject */

	/*# PlasmaRegisterType<MyProject.Services.Storage> */

	// this helps you to register all types from assembly
	/*# PlasmaRegisterAssembly<MyProject.Services.Storage> */

	// this helps you to register all types from current app domain
	/*# PlasmaRegisterAll */

	// this is required method to finally populate a code after all previous registrations
	/*# PlasmaGenerate */
}
```

Do not forget to reference all necessary libraries. Note that some of references are required for metacode execution, but descriptive compilation errors should help you to find it and their dependencies too.

After that you have to call
```
 			PlasmaRegistration.Run();
```
at any startup point, before first access to container... program.cs, or app.xaml.cs, or whatever.
You can call it multiple times if it is able to simplify your life (e.g. WCF behavior) (`TBD`)

# Result #

As a result you get a factory without any reflection.
This is how generated code looks like to get you an idea of how it works:

## Object Factory ##
```
Plasma.Internal.TypeFactoryRegister.Add<RoleManager>(c => new RoleManager());
Plasma.Internal.TypeFactoryRegister.Add<HeavyTransactionBuilder>(c => new HeavyTransactionBuilder(c.Get<IHeavyEntityBuilder>()));
Plasma.Internal.TypeFactoryRegister.Add<CurrentStateManager>(c => new CurrentStateManager(c.Get<ISwitchEventProvider>(), new Lazy<IStorage>(c.Get<IStorage>)));
```
## Plumbing ##
```
Plasma.Internal.TypeAutoPlumberRegister.Register<IdentifierService>((c, x)=>{
	x.NotificationPublisher = c.Get<INotificationPublisher>();
	x.IdentifierDao = c.Get<IIdentifierDao>();
	x.IdentifierProviderDao = c.Get<IIdentifierProviderDao>();
	x.PostingStatusManager = c.Get<IPostingStatusManager>();
	x.CustomInitialize(); // TBD e.g. for spring migrations
});
```
## Interface Implementations Lookup ##
From real life:
```
Plasma.Internal.FaceImplRegister.Register<Common.Model.Dao.IVendorCopyDao, Dal.Dao.HibernateVendorCopyDao>();

// Cannot register service for type 'ITransactionVisitor'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
#warning Cannot register service for type 'ITransactionVisitor'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
/*
Cannot register service for type 'ITransactionVisitor'. Specify instance, factory, use DefaultImplAttribute or just call class the same as interface
Inner: Impls of type 'Common.Model.Transaction.ITransactionVisitor': Transaction.TransactionCancelationVisitor, Common.Model.Transaction.TransactionVisitorAdapter, Common.Model.Accounting.AssetEventInventoryGenerator, Common.Model.Accounting.PositionEventInventoryGenerator, Common.Model.Accounting.QuantityEventInventoryGenerator, Common.Model.Accounting.LotEventInventoryGenerator, Common.Model.Accounting.SecondaryEventInventoryGenerator, Common.Model.Lite.Transaction.HeavyToLiteNewObjectFactory
*/
```