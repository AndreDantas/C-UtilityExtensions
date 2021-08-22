using System;

namespace UtilityExtensions.Core.Validations
{
    public class ValidationException : Exception
    {
        public string paramName;
        public string validationName;
        public string validationError;

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, string paramName) : base(message)
        {
            this.paramName = paramName;
        }

        public ValidationException(string message, string paramName, string validationError) : base(message)
        {
            this.paramName = paramName;
            this.validationError = validationError;
        }

        public ValidationException(string message, string paramName, string validationName, string validationError) : base(message)
        {
            this.paramName = paramName;
            this.validationError = validationError;
            this.validationName = validationName;
        }

        public ValidationException(string message, string paramName, Exception innerException) : base(message, innerException)
        {
            this.paramName = paramName;
        }

        public ValidationException(string message, string paramName, string validationError, Exception innerException) : base(message, innerException)
        {
            this.paramName = paramName;
            this.validationError = validationError;
        }

        public ValidationException(string message, string paramName, string validationName, string validationError, Exception innerException) : base(message, innerException)
        {
            this.paramName = paramName;
            this.validationError = validationError;
            this.validationName = validationName;
        }
    }
}