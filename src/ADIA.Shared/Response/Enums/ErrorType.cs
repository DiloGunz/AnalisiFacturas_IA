namespace ADIA.Shared.Response.Enums
{
    public record ResponseEnums
    {
        public enum ErrorType
        {
            None,
            Failure,
            Unexpected,
            Validation,
            Conflict,
            NotFound,
        }
    }
}
