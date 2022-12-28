namespace Auxquimia.Exceptions
{
    using System;

    /// <summary>
    /// Defines the <see cref="CustomException" />.
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public CustomException(string message) : base(message)
        {
        }
    }
}
