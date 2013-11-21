using System.Linq;
using System.Collections.Generic;
using System;
using MetaCreator;
using Plasma.Internal;

namespace Plasma.Meta
{
	internal class PlumbingStrategy : TypeScoutStrategy
	{
		private readonly StaticMining _mining;

		public PlumbingStrategy(StaticMining mining)
		{
			_mining = mining;
		}

		/// <summary>
		/// Predicate that determines whether a strategy going to work with this type
		/// </summary>
		public override bool Filter(Type type)
		{
			return type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition;
		}

		private readonly Dictionary<Type, bool> _result = new Dictionary<Type, bool>();

		/// <summary>
		/// Analyze type and yield all required type requests that are supposed to be directed to container
		/// </summary>
		public override IEnumerable<Type> GetRequestsCore(Type type)
		{
			var plumbingPros = PlasmaContainer.GetPlumbingProperties(type).ToArray();
			if (plumbingPros.Length > 0)
			{
				_result[type] = true;
				foreach (var pi in plumbingPros)
				{
					_mining.GetArgument(pi);
				}
			}
			else
			{
				_result[type] = false;
			}
			return Enumerable.Empty<Type>(); // requests from miner
		}

		/// <summary>
		/// Generate code necessary to prepare registers for working with types collected
		/// </summary>
		public override void Write(IMetaWriter writer)
		{
			foreach (var type in _result)
			{
				if (type.Value)
				{
					var plumbingPros = PlasmaContainer.GetPlumbingProperties(type.Key).ToArray();
					var plumbingc = "";
					foreach (var pi in plumbingPros)
					{
						plumbingc += string.Format("\tx.{0} = {1};\r\n", pi.Name, _mining.GetArgument(pi));
					}
					// todo remove extra cast in plumber
					// todo remove dependancy on 'c'
					writer.WriteLine(@"{2}.Register<{0}>((c, x)=>{{
{1}}});", type.Key, plumbingc, typeof(TypeAutoPlumberRegister));
				}
				else
				{
					writer.WriteLine("{1}.RegisterNone(typeof({0}));", type.Key, typeof(TypeAutoPlumberRegister));
				}
			}
		}
	}
}