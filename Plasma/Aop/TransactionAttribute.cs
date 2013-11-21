using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Spring.Transaction.Interceptor
{
	/// <summary>
	/// .NET Attribute for describing transactional behavior of methods in a class.
	/// </summary>
	public class TransactionAttribute : Plasma.Aop.TransactionAttribute
	{
		/// <summary>
		/// Initializes a new instance of the TransactionAttribute class.
		/// </summary>
		public TransactionAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the TransactionAttribute class.
		/// </summary>
		/// <param name="isolationLevel"></param>
		public TransactionAttribute(IsolationLevel isolationLevel) : base(isolationLevel)
		{
		}
	}
}

namespace Plasma.Aop
{
	/// <summary>
	/// .NET Attribute for describing transactional behavior of methods in a class.
	/// </summary>
	public class TransactionAttribute : Attribute
	{
		private readonly IsolationLevel _isolationLevel;

		/// <summary>
		/// Gets the isolation level.
		/// </summary>
		public IsolationLevel IsolationLevel
		{
			get { return _isolationLevel; }
		}

		/// <summary>
		/// Initializes a new instance of the TransactionAttribute class.
		/// </summary>
		public TransactionAttribute()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the TransactionAttribute class.
		/// </summary>
		/// <param name="isolationLevel"></param>
		public TransactionAttribute(IsolationLevel isolationLevel)
		{
			_isolationLevel = isolationLevel;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the transaction is readonly.
		/// </summary>
		public bool ReadOnly { get; set; }
	}
}
