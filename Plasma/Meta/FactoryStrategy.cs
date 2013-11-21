using System.Linq;
using System.Collections.Generic;
using System;
using MetaCreator;
using Plasma.Internal;

namespace Plasma.Meta
{
	internal class FactoryStrategy : TypeScoutStrategy
	{
		// TODO fix: TypeFactoryRegister.Add<Plasma.PlasmaContainer>(c => new Plasma.PlasmaContainer());

		private readonly StaticMining _mining;

		public FactoryStrategy(StaticMining mining)
		{
			_mining = mining;
		}

		private readonly Dictionary<Type, Result<string>> _result = new Dictionary<Type, Result<string>>();

		public override bool Filter(Type x)
		{
			return !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition && x.IsPublic && !typeof(Delegate).IsAssignableFrom(x);
		}

		public override IEnumerable<Type> GetRequestsCore(Type type)
		{
			try
			{
				var str = (string)_mining.CreateType(type);
				_result[type] = new Result<string>(str);
			}
			catch (StaticCompilerWarning ex)
			{
				_result[type] = new Result<string>(ex);
			}
			return Enumerable.Empty<Type>(); // requests from miner
		}

		public override void Write(IMetaWriter writer)
		{
			foreach (var result in _result)
			{
				if (result.Value.Res != null)
				{
					writer.WriteLine("{2}.Add<{0}>({1});", result.Key, result.Value.Res, typeof(TypeFactoryRegister));
				}
				else if (result.Value.Ex != null)
				{
					writer.WriteLine("#warning " + result.Value.Ex.Message);
					writer.WriteLine("/*");
					writer.WriteLine(ExceptionAnalyzer.ExceptionDetails(result.Value.Ex));
					writer.WriteLine("*/");
				}
			}
		}
	}
}