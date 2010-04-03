//-----------------------------------------------------------------------
// <copyright file="PriceRetrievalFailureException.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when a plugin fails to retrieve a price successfully.
    /// </summary>
    [Serializable]
    public class PriceRetrievalFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PriceRetrievalFailureException class.
        /// </summary>
        public PriceRetrievalFailureException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PriceRetrievalFailureException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PriceRetrievalFailureException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PriceRetrievalFailureException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PriceRetrievalFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PriceRetrievalFailureException class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
        protected PriceRetrievalFailureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
