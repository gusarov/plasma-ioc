using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlasmaTests.Sample
{
	public interface IPrivateIFace
	{
		
	}
	public class PrivateInner
	{
		private sealed class Private : IPrivateIFace
		{

		}
		protected class Protected
		{

		}
		internal class Internal
		{

		}
		protected internal class ProtectedInternal
		{

		}
	}
}
