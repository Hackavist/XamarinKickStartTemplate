using System;

namespace TemplateFoundation.Commands
{
    /// <summary>
    /// Represents errors that occur during IAsyncCommand execution.
    /// </summary>
    public class InvalidCommandParameterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.InvalidCommandParameterException"/> class.
        /// </summary>
        /// <param name="expectedType">Expected parameter type for AsyncCommand.Execute.</param>
        /// <param name="actualType">Actual parameter type for AsyncCommand.Execute.</param>
        public InvalidCommandParameterException(Type expectedType, Type actualType) : base(CreateErrorMessage(expectedType, actualType))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.InvalidCommandParameterException"/> class.
        /// </summary>
        /// <param name="message">Exception Message</param>
        public InvalidCommandParameterException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.InvalidCommandParameterException"/> class.
        /// </summary>
        public InvalidCommandParameterException()
        {

        }

        static string CreateErrorMessage(Type expectedType, Type actualType) =>
            $"Invalid type for parameter. Expected Type {expectedType}, but received Type {actualType}";
    }
}