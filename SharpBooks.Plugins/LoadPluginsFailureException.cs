// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System;

    /// <summary>
    /// Thrown when the PluginLoader encounters an error when loading from an assembly.
    /// </summary>
    [Serializable]
    public class LoadPluginsFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadPluginsFailureException"/> class.
        /// </summary>
        public LoadPluginsFailureException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadPluginsFailureException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LoadPluginsFailureException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadPluginsFailureException"/> class with a specified error.
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public LoadPluginsFailureException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadPluginsFailureException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
        protected LoadPluginsFailureException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
