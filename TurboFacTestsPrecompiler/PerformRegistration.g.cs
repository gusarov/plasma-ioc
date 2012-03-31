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
			
			
			
			// tbvfw2a0, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// TurboFacTests.Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
c.Add(new Lazy<IMyWorker>(()=>new MyWorker()));
c.Add(new Lazy<IMyService>(()=>new MyService()));
c.Add(new Lazy<IMyServiceWithOptionalArguments>(()=>new MyServiceWithOptionalArguments(c.Get<IMyStorage>(), c.Get<IMyWorker>())));
c.Add(new Lazy<IMyService3>(()=>new MyService3(c.Get<IMyPerformer>())));
c.Add(new Lazy<IMyPerformer>(()=>new MyPerformer()));

		}
	}
}
