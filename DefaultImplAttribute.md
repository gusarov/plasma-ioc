
```
[AttributeUsage(
   AttributeTargets.Interface
 | AttributeTargets.Class
 | AttributeTargets.Parameter
 | AttributeTargets.Property
, Inherited = false
, AllowMultiple = true
)]
public sealed class DefaultImplAttribute : Attribute
{
    public DefaultImplAttribute(Type type)
    {
        // ...
    }

    public DefaultImplAttribute(string assemblyQualifiedName)
    {
        // ...
    }

    // ...
}
```

Use this attribute to suggest default interface implementation for object containers.

# Remarks #

  * This attribute is placed in root namespace! You can use it without '`using`' imports
  * **Consider avoid this attribute, use only where absolutely required!** Plasma can understand single implementator and same-name implementator as a default implementation

# Example #

```
	[DefaultImpl(typeof(MyService))]
	public interface IMyService
	{
		int MyMethod();
	}
```

or

```
	[DefaultImpl("Plasma.Sample.MyService, Plasma.Sample"))]
	public interface IMyService
	{
		int MyMethod();
	}
```