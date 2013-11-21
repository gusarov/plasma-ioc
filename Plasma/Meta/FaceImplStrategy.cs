using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetaCreator;
using Plasma.Internal;

namespace Plasma.Meta
{
	class FaceImplStrategy : TypeScoutStrategy
	{
		public FaceImplStrategy(ReflectionMining mining)
		{
			_mining = mining;
		}

		private readonly Mining _mining;

		private readonly Dictionary<Type, Result<Type>> _result = new Dictionary<Type, Result<Type>>();

		public override bool Filter(Type type)
		{
			return type.IsInterface && type.IsPublic;
		}

		/// <summary>
		/// Analyze each type and yield all required type requests that are supposed to be directed to container
		/// </summary>
		public override IEnumerable<Type> GetRequestsCore(Type type)
		{
			Type defImpl = null;
			bool ok = false;
			try
			{
				defImpl = _mining.IfaceImpl(type);
				ok = true;
			}
			catch (Exception ex)
			{
				_result[type] = new Result<Type>(ex);
			}
			if (defImpl != null)
			{
				_result[type] = new Result<Type>(defImpl);
				yield return defImpl;
			}
		}

		/// <summary>
		/// Generate code necessary to prepare registers for working with types collected
		/// </summary>
		public override void Write(IMetaWriter writer)
		{
			foreach (var kvp in _result)
			{
				if (kvp.Value.Res != null)
				{
					writer.WriteLine("{2}.Register<{0}, {1}>();", kvp.Key, kvp.Value.Res, typeof(FaceImplRegister));
				}
				else if (kvp.Value.Ex != null)
				{
					writer.WriteLine("#warning " + kvp.Value.Ex.Message);
					writer.WriteLine("/*");
					writer.WriteLine(ExceptionAnalyzer.ExceptionDetails(kvp.Value.Ex));
					writer.WriteLine("*/");
				}
			}
		}
	}
}
