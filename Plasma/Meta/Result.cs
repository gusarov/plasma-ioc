using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma.Meta
{
	class Result<T>
	{
		private readonly Exception _ex;

		public Exception Ex
		{
			get { return _ex; }
		}

		private readonly T _result;

		public T Res
		{
			get { return _result; }
		}

		public Result(T result)
		{
			_result = result;
		}

		public Result(Exception ex)
		{
			_ex = ex;
		}
	}
}