// Copyright Gamelogic (c) http://www.gamelogic.co.za

using System;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// A version of NotImplementedException that takes the throwing type as argument. This is useful in class hierarchies
	/// where methods are meant to be overridden by derived types but cannot be made abstract (for example, because of 
	/// Unity limitations). The exception, when thrown, indicates which class should have implemented the method, but hasn't.
	/// </summary>
	/// <seealso cref="System.NotImplementedException" />
	/// <remarks>This is a develop-time exception, and should generally not be caught. </remarks>
	/// <example>In the following example, the derived class does not override the method Method. When Method is called
	/// on an instance of DerivedClass, a NotImplementedBy exception will be thrown with DerivedType as parameter,
	/// making it easy to see DerivedClass needs to implement Method. 
	/// 
	/// <code>
	/// [Abstract]
	/// public class BaseClass
	/// {
	///		[Abstract]
	///		public virtual void Method()
	///		{
	///			throw new NotImplementedBy(GetType());
	///		}
	/// }
	/// 
	/// public class DerivedClass : BaseClass { }
	/// </code>
	/// </example>
	public class NotImplementedByException : NotImplementedException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="NotImplementedByException"/> class.
		/// </summary>
		/// <param name="type">The type of the class that throws this exception.</param>
		public NotImplementedByException(Type type)
            : base("Not implemented by " + type)
        {

        }
    }
}
