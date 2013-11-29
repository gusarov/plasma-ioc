#if !PocketPC
using Plasma.Meta;
using MetaCreator;
#endif

#if !PocketPC
public static class PlasmaMetaRegisterExtensionRoot
{
	public static void RegisterAll(this IMetaWriter writer)
	{
		PlasmaMetaRegisterExtension.PlasmaRegisterAll(writer);
	}
}
#endif