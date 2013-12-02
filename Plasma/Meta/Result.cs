using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma.Meta
{
	internal class Result
	{
		public Exception Ex { get; set; }
	}

	internal class Result<T> : Result
	{
		public T Res { get; set; }
	}
}