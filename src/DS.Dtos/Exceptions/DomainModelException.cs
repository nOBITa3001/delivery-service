namespace DS.Dtos.Exceptions
{
    public class DomainModelException : ExceptionBase
    {
        public DomainModelException(params string[] errorMessages)
            : base(errorMessages)
        {
        }
    }
}
