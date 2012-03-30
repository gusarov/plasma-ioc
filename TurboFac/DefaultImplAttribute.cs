using System;

//namespace TurboFac
//{
	namespace TurboFac
	{
		//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Impl"), AttributeUsage(AttributeTargets.Interface|AttributeTargets.Class|AttributeTargets.Parameter|AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
		public sealed class DefaultImplAttribute : Attribute
		{
			Type _type;
			readonly string _typeAqn;

			public DefaultImplAttribute(Type type)
			{
				_type = type;
			}

			public DefaultImplAttribute(string assemblyQualifiedName)
			{
				_typeAqn = assemblyQualifiedName;
			}

			public Type TargetType
			{
				get { return _type ?? ( _type = Type.GetType(_typeAqn, true)); }
			}
		}
	}
//}
