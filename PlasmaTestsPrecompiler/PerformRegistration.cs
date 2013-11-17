using System;
using System.Collections.Generic;
using System.Linq;
using PlasmaTests.Sample;
using Plasma.Meta;
using Plasma.Internal;

using PlasmaTests;
using PlasmaTests.Sample;
using PlasmaTests.Sample.Proxy;

namespace PlasmaTests.Precompiler
{
	/*+ static object ref1 = typeof(IMyWorker); */
	/* + static object ref2 = typeof(Spring.Objects.Factory.Config.ObjectFactoryCreatingFactoryObject); */

	/*@ errorremap off */
	/*@ FileInProject */
	/*@ CSharpVersion v4.0 */
	/*@ debugLogging true */

	/*# PlasmaNullObject true */
	/*# PlasmaProxy true */

	/*# PlasmaRegisterAll */
	/*# PlasmaGenerate */

}
