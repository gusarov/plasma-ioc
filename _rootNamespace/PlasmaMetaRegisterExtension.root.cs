using MetaCreator;

using Plasma.Meta;

public static class PlasmaMetaRegisterExtensionRoot
{
	public static void RegisterAll(this IMetaWriter writer)
	{
		PlasmaMetaRegisterExtension.RegisterAll(writer);
	}
}

