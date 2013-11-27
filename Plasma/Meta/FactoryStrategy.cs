using System.Linq;
using System.Collections.Generic;
using System;
using MetaCreator;
using Plasma.Internal;

namespace Plasma.Meta
{
	internal class FactoryStrategy : TypeScoutStrategy
	{
		class FactoryResult : Result
		{
			public FactoryResult(Exception ex)
				: base(ex)
			{
				
			}

			public FactoryResult()
				: base(null)
			{
				
			}

			/// <summary>
			/// usual code
			/// </summary>
			public string FactoryCode { get; set; }

			/// <summary>
			/// Specify that this type have a custom factory
			/// </summary>
			public bool GeneratedByFactory
			{
				get { return FactoryFactoryCode != null; }
			}

			/// <summary>
			/// Code that delegates creation to IFactory that is detected
			/// </summary>
			public string FactoryFactoryCode { get; set; }
		}

		// TODO fix: TypeFactoryRegister.Add<Plasma.PlasmaContainer>(c => new Plasma.PlasmaContainer());
		// TODO fix: TypeFactoryRegister.Add<MySubGroup>(c => new MySubGroup(c.Get<Plasma.IPlasmaProvider>()));
		private readonly StaticMining _mining;

		public FactoryStrategy(StaticMining mining)
		{
			_mining = mining;
		}

		private readonly Dictionary<Type, FactoryResult> _result = new Dictionary<Type, FactoryResult>();

		public override bool Filter(Type x)
		{
			return !x.IsAbstract && !x.IsInterface && !x.IsGenericTypeDefinition && x.IsPublic && !typeof(Delegate).IsAssignableFrom(x);
		}

		public override IEnumerable<Type> GetRequestsCore(Type type)
		{
			try
			{
				var str = (string)_mining.CreateType(type);
				var result = ResultFor(type);
				result.FactoryCode = str;

				if (typeof(IFactory).IsAssignableFrom(type))
				{
					// get IFactory<T>
					var iface = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IFactory<,>));
					if (iface != null)
					{
						var args = iface.GetGenericArguments();
						var targetRequest = args[0];
						var targetResult = args[1];
						result = ResultFor(targetResult);
						result.FactoryFactoryCode = string.Format("c => c.Get<{0}>().Create()", type.CSharpTypeIdentifier());
					}
				}
			}
			catch (StaticCompilerWarning ex)
			{
				_result[type] = new FactoryResult(ex);
			}
			return Enumerable.Empty<Type>(); // requests from miner
		}

		FactoryResult ResultFor(Type type)
		{
			FactoryResult result;
			if (!_result.TryGetValue(type, out result))
			{
				_result[type] = result = new FactoryResult();
			}
			return result;
		}

		public override void Write(IMetaWriter writer)
		{
			foreach (var result in _result)
			{
				if (result.Value.FactoryCode != null || result.Value.FactoryFactoryCode != null)
				{
					writer.WriteLine("{2}.Add<{0}>({1});", result.Key, result.Value.FactoryFactoryCode ?? result.Value.FactoryCode, typeof(TypeFactoryRegister));
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