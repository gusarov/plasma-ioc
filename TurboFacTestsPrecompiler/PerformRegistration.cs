using System;
using System.Collections.Generic;
using System.Linq;
using TurboFac;
using TurboFac.Meta;
using TurboFacTests.Sample;
	
namespace TurboFacTests.Precompiler
{
	public static class PerformRegistration
	{
		public static void Perform(ITurboContainer c)
		{
			/*+ static object ref1 = typeof(IMyWorker); */
			/*@ errorremap off */
			/*@ FileInProject */
			/*# RegisterAll */
		}
	}
}
