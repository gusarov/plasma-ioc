using System.Linq;
using System.Collections.Generic;
using System;

namespace Plasma.Meta
{
	internal class Result
	{
		private readonly Exception _ex;

		public Exception Ex
		{
			get { return _ex; }
		}

		public Result(Exception ex)
		{
			_ex = ex;
		}
	}

	internal class Result<T> : Result
	{
		private readonly T _result;

		public T Res
		{
			get { return _result; }
		}

		public Result(T result) : base(null)
		{
			_result = result;
		}

		public Result(Exception ex) : base(ex)
		{
		}
	}
}