using System;
using System.Reflection;
using System.Runtime.Serialization;
using AopAlliance.Aop;

namespace AopAlliance.Intercept
{
    public interface IConstructorInterceptor : IInterceptor, IAdvice
    {
        // Methods
        object Construct(IConstructorInvocation invocation);
    }

    public interface IConstructorInvocation : IInvocation, IJoinpoint
    {
        // Properties
        ConstructorInfo Constructor { get; }
    }

    public interface IInterceptor : IAdvice
    {
    }

    public interface IInvocation : IJoinpoint
    {
        // Properties
        object[] Arguments { get; }
    }

    public interface IJoinpoint
    {
        // Methods
        object Proceed();

        // Properties
        MemberInfo StaticPart { get; }
        object This { get; }
    }

    public interface IMethodInterceptor : IInterceptor, IAdvice
    {
        // Methods
        object Invoke(IMethodInvocation invocation);
    }

    public interface IMethodInvocation : IInvocation, IJoinpoint
    {
        // Properties
        MethodInfo Method { get; }
        object Proxy { get; }
        object Target { get; }
        Type TargetType { get; }
    }
}
namespace AopAlliance.Aop
{
	[Serializable]
	public class AspectException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public AspectException()
		{
		}

		public AspectException(string message) : base(message)
		{
		}

		public AspectException(string message, Exception inner) : base(message, inner)
		{
		}

		protected AspectException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}

	public interface IAdvice
	{
	}
}



 
