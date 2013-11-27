using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plasma;

namespace PlasmaTests.Sample
{
	public interface ISession
	{
		string Config { get; }
	}

	public class SessionFactory : Factory<ISession, Session>
	{
		/// <summary>
		/// Create an object
		/// </summary>
		public override ISession Create()
		{
			return new Session("factored");
		}
	}

	public class Session : ISession
	{
		private readonly string _config;

		public string Config
		{
			get { return _config; }
		}

		public Session(string config)
		{
			_config = config;
		}
	}
}
