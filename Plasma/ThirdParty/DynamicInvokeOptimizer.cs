using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Plasma.ThirdParty
{
	static class DynamicInvokeOptimizer
	{
		#region HideObjectMembers
		
		/// <summary>Do not call this method.</summary>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "b")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "a")]
		[Obsolete("Do not call this method", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new static bool Equals(object a, object b)
		{
			throw new InvalidOperationException("Do not call this method");
		}

		/// <summary>Do not call this method.</summary>
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "b")]
		[SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "a")]
		[Obsolete("Do not call this method", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new static bool ReferenceEquals(object a, object b)
		{
			throw new InvalidOperationException("Do not call this method");
		} 
	#endregion

		static readonly Dictionary<Type, Type> _mapDelegateToUnboundDelegate = new Dictionary<Type, Type>();

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Type GetUnboundDelegate(Delegate deleg)
		{
			if (deleg == null)
			{
				throw new ArgumentNullException("deleg");
			}
			var delegateType = deleg.GetType();
			if(!typeof(Delegate).IsAssignableFrom(delegateType))
			{
				throw new ArgumentException("delegateType is not Delegate");
			}
			Type unbound;
			if (!_mapDelegateToUnboundDelegate.TryGetValue(delegateType, out unbound))
			{
				var sig = GetDelegateSignature(deleg);
				var ret = sig.Item1;
				var args = new[] {typeof(object)}.Concat(sig.Item2).ToArray();
				_mapDelegateToUnboundDelegate[delegateType] = unbound = GetDelegateTypeFromSignature(ret, args);
			}
			return unbound;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Tuple<Type, Type[]> GetDelegateSignature(Delegate deleg)
		{
			if (deleg == null)
			{
				throw new ArgumentNullException("deleg");
			}
			return Tuple.Create( deleg.Method.ReturnType, deleg.Method.GetParameters().Select(x=>x.ParameterType).ToArray());
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Type GetDelegateTypeFromSignature(Type returnType, params Type[] args)
		{
			if (returnType == null || returnType == typeof(void))
			{
				if (args.Length == 0)
				{
					return typeof(Action);
				}
				var gen = Type.GetType("System.Action`" + args.Length + ", mscorlib", true);
				var type = gen.MakeGenericType(args);
				return type;
			}
			else
			{
				var gen = Type.GetType("System.Func`" + (args.Length + 1) + ", mscorlib", true);
				var type = gen.MakeGenericType(args.Concat(new[] {returnType}).ToArray());
				return type;
			}
		}

		static readonly Dictionary<MethodInfo, Delegate> _dic = new Dictionary<MethodInfo, Delegate>();

		public static TDelegate Compile<TTarget, TDelegate>(string methodName) where TDelegate : class
		{
			return Compile<TDelegate>(typeof(TTarget), methodName);
		}

		public static TDelegate Compile<TDelegate>(Type type, string methodName) where TDelegate : class
		{
			return Compile<TDelegate>(type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic));
		}

		/// <summary>
		/// Create lightweight method and delegate to that method.
		/// For instance methods define another first object parameter in delegate.
		/// </summary>
		/// <typeparam name="TDelegate"></typeparam>
		/// <param name="methodInfo"></param>
		/// <returns>Compiled strongly typed delegate</returns>
		/// <example>
		/// <code>
		/// class TestClass
		/// {
		///     public int TestMethod(int arg)
		///     {
		///         return arg * arg;
		///     }
		/// }
		/// 
		/// class TestInvokator
		/// {
		///     void Main()
		///     {
		///         typeof(TestClass).GetMethod("TestMethod").Compile&lt;Func&lt;object, int, int&gt;&gt;
		///     }
		/// }
		/// </code>
		/// </example>
		public static TDelegate Compile<TDelegate>(this MethodInfo methodInfo) where TDelegate : class
		{
			return (TDelegate)(object)Compile(methodInfo, typeof(TDelegate));
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Delegate Compile(MethodInfo methodInfo, Type unboundDelegateType)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			Delegate del;
			if (!_dic.TryGetValue(methodInfo, out del))
			{
				var argsList = new List<Type>();
				if (!methodInfo.IsStatic)
				{
					// next sample lead us to only one acceptable bounding object type. Wtih 'object' this signature delegate can be bound to any classes
					argsList.Add(typeof(object));
					// and such approach lead to problems, because a first who bound with Action<X, a,b,c> register this dynamic method and next call Action<Y, a,b,c> will fail.
					// T O D O How about custom delegates instead of Action<int>. How about Func with return value at first argument?
					// argsList.Add(unboundDelegateType.GetGenericArguments()[0]);
				}
				argsList.AddRange(methodInfo.GetParameters().Select(x => x.ParameterType));
				var args = argsList.ToArray();
				var dm = new DynamicMethod(string.Empty, methodInfo.ReturnType, args, typeof(DynamicInvokeOptimizer), true);
				var il = dm.GetILGenerator();
				for (int i = 0; i < args.Length; i++)
				{
					il.Emit(OpCodes.Ldarg, i);
				}
				il.Emit(methodInfo.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, methodInfo);
//				if (methodInfo.ReturnType == (typeof(void)))
//				{
//				    il.Emit(OpCodes.Ldnull);
//				}
				il.Emit(OpCodes.Ret);
				del = dm.CreateDelegate(unboundDelegateType);
				_dic.Add(methodInfo, del);
			}
			return del;
		}
	}

}
