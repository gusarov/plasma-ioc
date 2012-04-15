using System;
using System.Collections.Generic;
using System.Linq;

using Plasma;

using PlasmaTests.Sample;

namespace PlasmaTests.Precompiler
{
	public static class PerformRegistration
	{
		public static void Perform(IPlasmaContainer c)
		{
			/*+ static object ref1 = typeof(IMyWorker); */
			/*@ errorremap off */
			/*@ FileInProject */
			/*# RegisterAll */
		}
	}
}
