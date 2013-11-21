using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using AopAlliance.Aop;
using AopAlliance.Intercept;

namespace Spring.Aop.Support
{

	[Serializable]
	public abstract class StaticMethodMatcherPointcut : StaticMethodMatcher, IPointcut
	{
		// Fields
		private ITypeFilter typeFilter = TrueTypeFilter.True;

		// Methods
		protected StaticMethodMatcherPointcut()
		{
		}

		// Properties
		public virtual IMethodMatcher MethodMatcher
		{
			get { return this; }
		}

		public virtual ITypeFilter TypeFilter
		{
			get { return this.typeFilter; }
			set { this.typeFilter = value; }
		}
	}

	[Serializable]
	public abstract class StaticMethodMatcher : IMethodMatcher, IDeserializationCallback
	{
		// Methods
		protected StaticMethodMatcher()
		{
		}

		public abstract bool Matches(MethodInfo method, Type targetType);

		public bool Matches(MethodInfo method, Type targetType, object[] args)
		{
			throw new NotSupportedException("Illegal IMethodMatcher usage. Cannot call 3-arg Matches method on a static matcher.");
		}

		protected virtual void OnDeserialization(object sender)
		{
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialization(sender);
		}

		// Properties
		public bool IsRuntime
		{
			get { return false; }
		}
	}
}

namespace Spring.Aop
{
	public interface IAdvisor
	{
		// Properties
		IAdvice Advice { get; }
		bool IsPerInstance { get; }
	}

	public interface IAdvisors
	{
		// Properties
		IList Advisors { get; set; }
	}

	public interface IAfterReturningAdvice : IAdvice
	{
		// Methods
		void AfterReturning(object returnValue, MethodInfo method, object[] args, object target);
	}

	public interface IBeforeAdvice : IAdvice
	{
	}

	public interface IIntroductionAdvisor : IAdvisor
	{
		// Methods
		void ValidateInterfaces();

		// Properties
		Type[] Interfaces { get; }
		ITypeFilter TypeFilter { get; }
	}

	public interface IIntroductionInterceptor : IMethodInterceptor, IInterceptor, IAdvice
	{
		// Methods
		bool ImplementsInterface(Type intf);
	}

	public interface IMethodBeforeAdvice : IBeforeAdvice, IAdvice
	{
		// Methods
		void Before(MethodInfo method, object[] args, object target);
	}

	public interface IMethodMatcher
	{
		// Methods
		bool Matches(MethodInfo method, Type targetType);
		bool Matches(MethodInfo method, Type targetType, object[] args);

		// Properties
		bool IsRuntime { get; }
	}

	public interface IPointcut
	{
		// Properties
		IMethodMatcher MethodMatcher { get; }
		ITypeFilter TypeFilter { get; }
	}

	public interface IPointcutAdvisor : IAdvisor
	{
		// Properties
		IPointcut Pointcut { get; }
	}

	public interface ITargetSource
	{
		// Methods
		object GetTarget();
		void ReleaseTarget(object target);

		// Properties
		bool IsStatic { get; }
		Type TargetType { get; }
	}

	public interface IThrowsAdvice : IAdvice
	{
	}

	public interface ITypeFilter
	{
		// Methods
		bool Matches(Type type);
	}

	[Serializable]
	public sealed class TrueMethodMatcher : IMethodMatcher, ISerializable
	{
		// Fields
		public static readonly IMethodMatcher True = new TrueMethodMatcher();

		// Methods
		private TrueMethodMatcher()
		{
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(TrueMethodMatcherObjectReference));
		}

		public bool Matches(MethodInfo method, Type targetType)
		{
			return true;
		}

		public bool Matches(MethodInfo method, Type targetType, object[] args)
		{
			return true;
		}

		public override string ToString()
		{
			return "TrueMethodMatcher.True";
		}

		// Properties
		public bool IsRuntime
		{
			get { return false; }
		}

		// Nested Types
		[Serializable]
		private sealed class TrueMethodMatcherObjectReference : IObjectReference
		{
			// Methods
			public object GetRealObject(StreamingContext context)
			{
				return TrueMethodMatcher.True;
			}
		}
	}



	[Serializable]
	public sealed class TruePointcut : IPointcut, ISerializable
	{
		// Fields
		public static readonly IPointcut True = new TruePointcut();

		// Methods
		private TruePointcut()
		{
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(TruePointcutObjectReference));
		}

		public override string ToString()
		{
			return "TruePointcut.TRUE";
		}

		// Properties
		public IMethodMatcher MethodMatcher
		{
			get { return TrueMethodMatcher.True; }
		}

		public ITypeFilter TypeFilter
		{
			get { return TrueTypeFilter.True; }
		}

		// Nested Types
		[Serializable]
		private sealed class TruePointcutObjectReference : IObjectReference
		{
			// Methods
			public object GetRealObject(StreamingContext context)
			{
				return TruePointcut.True;
			}
		}
	}

	[Serializable]
	public sealed class TrueTypeFilter : ITypeFilter, ISerializable
	{
		// Fields
		public static readonly ITypeFilter True = new TrueTypeFilter();

		// Methods
		private TrueTypeFilter()
		{
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(TrueTypeFilterObjectReference));
		}

		public bool Matches(Type type)
		{
			return true;
		}

		public override string ToString()
		{
			return "TrueTypeFilter.True";
		}

		// Nested Types
		[Serializable]
		private sealed class TrueTypeFilterObjectReference : IObjectReference
		{
			// Methods
			public object GetRealObject(StreamingContext context)
			{
				return TrueTypeFilter.True;
			}
		}
	}


}

