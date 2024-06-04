using System;

namespace ApiTFG.Exceptions
{
    public class PostException : Exception
    {
        public PostException() : base()
        {
        }

        public PostException(string message) : base(message)
        {
        }

        public PostException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}