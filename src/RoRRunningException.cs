using System;

namespace SMAC
{
    /// <summary>
    /// The exception that is thrown when Rigs of Rods is running.
    /// </summary>
    public class RoRRunningException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoRRunningException"/> class.
        /// </summary>
        public RoRRunningException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoRRunningException"/> class
        /// with a custom message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public RoRRunningException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoRRunningException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception.
        /// If the innerException parameter is not null, the current exception is raised
        /// in a catch block that handles the inner exception.
        /// </param>
        public RoRRunningException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}