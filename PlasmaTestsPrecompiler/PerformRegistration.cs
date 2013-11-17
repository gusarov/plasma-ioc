using System;
using System.Collections.Generic;
using System.Linq;
using PlasmaTests.Sample;
using Plasma.Meta;

namespace PlasmaTests.Precompiler
{
	/*+ static object ref1 = typeof(IMyWorker); */
	/* + static object ref2 = typeof(Spring.Objects.Factory.Config.ObjectFactoryCreatingFactoryObject); */

	/*@ errorremap off */
	/*@ FileInProject */
	/*@ CSharpVersion v4.0 */

	/*# PlasmaNullObject true */
	/*# PlasmaProxy true */

	/*# PlasmaRegisterAll */
	/*# PlasmaGenerate */

}
