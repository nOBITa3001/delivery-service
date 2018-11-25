namespace DS.Dtos.Exceptions
{
    public class InvalidArgumentException : ExceptionBase
    {
        public InvalidArgumentException(params string[] errorMessages)
            : base(errorMessages)
        {
        }
    }
}
