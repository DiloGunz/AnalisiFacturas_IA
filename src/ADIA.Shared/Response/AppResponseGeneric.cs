using ADIA.Shared.Response.Enums;
using System.Text.Json.Serialization;

namespace ADIA.Shared.Response
{
    public record AppResponse<T> : AppResponseBase// where T : class
    {
        [JsonPropertyOrder(2)]
        public T Result { get; set; } = default!;

        public static AppResponse<T> CreateDefault()
        {
            return new AppResponse<T>().Failure();
        }


        private AppResponse<T> SetErrorType(ResponseEnums.ErrorType errorType, string m = "", T data = default!)
        {
            ErrorType = errorType;
            Result = data;
            PrepareResponse(false, string.IsNullOrWhiteSpace(m) ? new List<string>() : new List<string>() { m });
            return this;
        }

        private AppResponse<T> SetErrorType(ResponseEnums.ErrorType errorType, IEnumerable<string> m, T data = default!)
        {
            ErrorType = errorType;
            Result = data;
            PrepareResponse(false, m ?? new List<string>());
            return this;
        }

        public AppResponse<T> Success(T data = default!, string m = "")
        {
            ErrorType = ResponseEnums.ErrorType.None;
            PrepareResponse(true, string.IsNullOrWhiteSpace(m) ? new List<string>() : new List<string>() { m });
            Result = data;
            return this;
        }

        public AppResponse<T> Failure(string m = "", T data = default!) => SetErrorType(ResponseEnums.ErrorType.Failure, m, data);
        public AppResponse<T> Validation(string m = "", T data = default!) => SetErrorType(ResponseEnums.ErrorType.Validation, m, data);
        public AppResponse<T> NotFound(string m = "", T data = default!) => SetErrorType(ResponseEnums.ErrorType.NotFound, m, data);
        public AppResponse<T> Unexpected(string m = "", T data = default!) => SetErrorType(ResponseEnums.ErrorType.Unexpected, m, data);
        public AppResponse<T> Conflict(string m = "", T data = default!) => SetErrorType(ResponseEnums.ErrorType.Conflict, m, data);


        public AppResponse<T> Failure(IEnumerable<string> m, T data = default!) => SetErrorType(ResponseEnums.ErrorType.Failure, m, data);
        public AppResponse<T> Validation(IEnumerable<string> m, T data = default!) => SetErrorType(ResponseEnums.ErrorType.Validation, m, data);
        public AppResponse<T> NotFound(IEnumerable<string> m, T data = default!) => SetErrorType(ResponseEnums.ErrorType.NotFound, m, data);
        public AppResponse<T> Unexpected(IEnumerable<string> m, T data = default!) => SetErrorType(ResponseEnums.ErrorType.Unexpected, m, data);
        public AppResponse<T> Conflict(IEnumerable<string> m, T data = default!) => SetErrorType(ResponseEnums.ErrorType.Conflict, m, data);
    }
}
