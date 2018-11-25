namespace DS.Dtos.Exceptions
{
    public class DataAccessNotFoundException : ExceptionBase
    {
        public DataAccessNotFoundException(params string[] errorMessages)
            : base(errorMessages)
        {
        }
    }
}
