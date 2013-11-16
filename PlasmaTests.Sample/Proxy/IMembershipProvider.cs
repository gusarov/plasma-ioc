using System.Collections.Generic;

namespace PlasmaTests.Sample.Proxy
{
	public interface IMembershipProvider
	{
		bool ValidateUser(string login, string password);
		IEnumerable<string> ListUsers();
		IList<string> ListUsers2();
		void AddUser(string login, string password);
		void DeleteUser(string login);
		byte[] TestArray();
	}
}