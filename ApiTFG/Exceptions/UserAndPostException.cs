namespace ApiTFG.Exceptions
{
    public class UserAndPostException : Exception
    {
        public UserAndPostException() { }

        public UserAndPostException(string message) : base(message) { }

        public UserAndPostException(string message, Exception innerException) : base(message, innerException) { }
    }
}