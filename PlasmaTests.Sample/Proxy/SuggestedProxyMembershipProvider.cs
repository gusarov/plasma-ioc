using System.Collections.Generic;

using Plasma.Proxy;

namespace PlasmaTests.Sample.Proxy
{
	public class SuggestedProxyMembershipProvider : ProxyBase<IMembershipProvider>
	{
		public SuggestedProxyMembershipProvider(IMembershipProvider originalObject) : base(originalObject)
		{
		}

		#region IMembershipProvider Members

		public virtual bool ValidateUser(string login, string password)
		{
			return Original.ValidateUser(login, password);
		}

		public virtual IEnumerable<string> ListUsers()
		{
			return Original.ListUsers();
		}

		public virtual void AddUser(string login, string password)
		{
			Original.AddUser(login, password);
		}

		public virtual void DeleteUser(string login)
		{
			Original.DeleteUser(login);
		}

		#endregion
	}
}
