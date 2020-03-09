using System;
using System.Collections.Generic;
using System.Text;

namespace JsonValueExtractor_Validator.Exceptions
{
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException()
        {
        }

        public InvalidJsonException(string message)
            : base(message)
        {
        }

        public InvalidJsonException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
