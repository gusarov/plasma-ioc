using System;
using System.Linq;

using MetaCreator;
using MetaCreator.Extensions;

using TurboFac;
using TurboFac.ThirdParty;

//namespace TurboFac.MetaRegister
//{
public static class TurboFacMetaRegisterExtension
{
	public static void RegisterAll(this IMetaWriter writer)
	{
		var typesAll = AppDomain.CurrentDomain.GetAssemblies();//.SelectMany(x => x.GetTypes());

		foreach (var type in typesAll)
		{
			writer.WriteLine("// "+type.FullName);
			// writer.WriteLine("// c.Add<{0}>();", MixinExtension.CSharpTypeIdentifier(type));
		}

		var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => UtilsExt.Attribute<TurboFacAttribute>(x) != null);
		foreach (var type in types)
		{
			writer.WriteLine("c.Add<{0}>();", MixinExtension.CSharpTypeIdentifier(type));
		}
	}
}
//}
