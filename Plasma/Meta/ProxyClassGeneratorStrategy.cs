using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Plasma.Meta
{
	class ProxyClassGeneratorStrategy : ClassGeneratorStrategy
	{
		protected override string Prefix
		{
			get { return "Proxy"; }
		}

		protected override string BaseClass
		{
			get { return "Plasma.Proxy.ProxyBase<{0}>, "; }
		}

		protected override void BeforeMembers()
		{
			_writer.Write("\tpublic {0}({1} originalObject) : base(originalObject)", _typeShortName, _type.CSharpTypeIdentifier());
			_writer.Write("\t{");
			_writer.WriteLine("\t}");
		}

		protected override void WriteMethodBody(MethodInfo method)
		{
			_writer.Write("{0}Original.{1}({2});", method.ReturnType == typeof(void) ? "" : "return ", method.Name, string.Join(", ", method.GetParameters().Select(x => x.Name).ToArray()));
		}
		protected override void WriteEventSubscriber(EventInfo eventInfo)
		{
			_writer.Write("Original.{0} += value;", eventInfo.Name);
		}
		protected override void WriteEventUnsubscriber(EventInfo eventInfo)
		{
			_writer.Write("Original.{0} -= value;", eventInfo.Name);
		}
		protected override void WritePropertyGetter(PropertyInfo pro)
		{
			_writer.Write("return Original.{0};", pro.Name);
		}
		protected override void WriteIndexerGetter(PropertyInfo pro, string indexerArguments)
		{
			_writer.Write("return Original{0};", indexerArguments);
		}
		protected override void WritePropertySetter(PropertyInfo pro)
		{
			_writer.Write("Original.{0} = value;", pro.Name);
		}
		protected override void WriteIndexerSetter(PropertyInfo pro, string indexerArguments)
		{
			_writer.Write("Original{0} = value;", indexerArguments);
		}
	}
}