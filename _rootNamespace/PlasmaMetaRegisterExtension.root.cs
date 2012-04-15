using MetaCreator;

using Plasma.Meta;

public static class PlasmaMetaRegisterExtensionRoot
{
	public static void RegisterAll(this IMetaWriter writer)
	{
		RegisterAll(writer, "c");
	}

	public static void RegisterAll(this IMetaWriter writer, string containerVariableName)
	{
		PlasmaMetaRegisterExtension.RegisterAll(writer, containerVariableName);
	}
}

