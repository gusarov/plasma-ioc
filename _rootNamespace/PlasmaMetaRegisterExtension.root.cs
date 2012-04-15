using MetaCreator;

using Plasma.Meta;

public static class PlasmaMetaRegisterExtensionRoot
{
	public static void RegisterAll(this IMetaWriter writer, string containerVariableName = "c")
	{
		PlasmaMetaRegisterExtension.RegisterAll(writer, containerVariableName);
	}
}

