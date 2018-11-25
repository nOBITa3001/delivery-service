namespace DS.Dtos.ResponseMessages
{
    public partial class ResponseMessages
    {
        public static class Validation
        {
            public const string StartRequired = "Start cannot be null or empty.";
            public const string EndRequired = "End cannot be null or empty.";
            public const string RangeInvalidTemplate = "{0} must be between {1} and {2}.";
        }
    }
}
