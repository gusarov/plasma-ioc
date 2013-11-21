using System.Linq;
using System.Collections.Generic;
using System;

namespace PlasmaTests.Sample
{
	public class GenericPerformer
	{
		private readonly HibernateCrudDao<IMyService> _sdao1;

		public HibernateCrudDao<IMyService> Sdao1
		{
			get { return _sdao1; }
		}

		[Inject]
		public HibernateCrudDao<IMyService2> Sdao2 { get; set; }

		public HibernateCrudDao<IMyService3> Sdao3 { private get; set; }

		public HibernateCrudDao<IMyService3> Sdao3_ { get { return Sdao3; } }

		public GenericPerformer(HibernateCrudDao<IMyService> sdao1)
		{
			_sdao1 = sdao1;
		}
	}
}