using System.Collections.Generic;

namespace PlasmaTests.Sample.Proxy
{
	public interface IMembershipProvider
	{
		bool ValidateUser(string login, string password);
		IEnumerable<string> ListUsers();
		void AddUser(string login, string password);
		void DeleteUser(string login);
	}
}