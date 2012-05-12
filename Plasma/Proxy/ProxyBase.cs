using System;

namespace Plasma.Proxy
{
	public class ProxyBase<T> where T : class
	{
		protected T Original { get; private set; }

		public ProxyBase(T originalObject)
		{
			if (originalObject == null)
			{
				throw new ArgumentNullException("originalObject");
			}
			Original = originalObject;
		}

	}
}