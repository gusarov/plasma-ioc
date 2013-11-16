using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PlasmaTests.Sample.Proxy
{
	public interface ISimpleDataForStubbing
	{
		int Test { get; set; }
	}

	public interface IComplexStubbing : INotifyPropertyChanged
	{
		int M(long p, string q);
		int Test1 { get; set; }
		int Test2 { get; }
		int Test3 { set; }
		Action DelegateProperty { get; set; }
		event Action Event;
		string this[Guid g, int r] { get; set; }
	}

	public interface IComplexStubbingDerived : IComplexStubbing
	{
	}

	public interface IComplexStubbingDerived2 : IComplexStubbingDerived
	{
	}

	public interface IComplexStubbingDerived3 : IComplexStubbingDerived2
	{
	}
}
