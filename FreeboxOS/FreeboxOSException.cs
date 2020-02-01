using System;

namespace FreeboxOS
{
    /// <summary>
    /// Freebox OS custom exception
    /// </summary>
    public class FreeboxOSException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeboxOSException"/> class
        /// </summary>
        public FreeboxOSException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeboxOSException"/> class
        /// </summary>
        /// <param name="message">the message that describes the error</param>
        public FreeboxOSException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeboxOSException"/> class
        /// </summary>
        /// <param name="errorCode">error code</param>
        /// <param name="message">the message that describes the error</param>
        public FreeboxOSException(string errorCode, string message) : this(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeboxOSException"/> class
        /// </summary>
        /// <param name="errorCode">error code</param>
        /// <param name="message">the message that describes the error</param>
        /// <param name="innerException">the exception that is the cause of the current exception, or a null reference if no inner exception is specified</param>
        public FreeboxOSException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code
        /// </summary>
        public string ErrorCode { get; }
    }
}
