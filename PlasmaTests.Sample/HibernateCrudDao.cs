using System.Linq;
using System.Collections.Generic;
using System;

namespace PlasmaTests.Sample
{
	public class HibernateCrudDao<T>
	{
		[Inject]
		public ISession Session { get; set; }

		public void Create(T item)
		{

		}
		public T Read(int id)
		{
			return default(T);
		}
		public void Updete(T item)
		{

		}
		public void Delete(T item)
		{
			
		}
	}
}