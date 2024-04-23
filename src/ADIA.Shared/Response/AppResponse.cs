using ADIA.Shared.Response.Enums;
using System.Text.Json.Serialization;

namespace ADIA.Shared.Response
{
    public record AppResponse : AppResponseBase
    {
        [JsonPropertyOrder(2)]
        public dynamic Result { get; set; } = null!;


        public static AppResponse CreateDefault()
        {
            return new AppResponse().Failure();
        }

        private AppResponse SetErrorType(ResponseEnums.ErrorType errorType, string m = "", dynamic data = default!)
        {
            ErrorType = errorType;
            Result = data;
            PrepareResponse(false, string.IsNullOrWhiteSpace(m) ? new List<string>() : new List<string>() { m });
            return this;
        }

        private AppResponse SetErrorType(ResponseEnums.ErrorType errorType, IEnumerable<string> m, dynamic data = default!)
        {
            ErrorType = errorType;
            Result = data;
            PrepareResponse(false, m ?? new List<string>());
            return this;
        }


        public AppResponse Success(dynamic data = default!, string m = "")
        {
            ErrorType = ResponseEnums.ErrorType.None;
            PrepareResponse(true, string.IsNullOrWhiteSpace(m) ? new List<string>() : new List<string>() { m });
            Result = data;
            return this;
        }

        public AppResponse Failure(string m = "", dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Failure, m, data);
        public AppResponse Validation(string m = "", dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Validation, m, data);
        public AppResponse NotFound(string m = "", dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.NotFound, m, data);
        public AppResponse Unexpected(string m = "", dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Unexpected, m, data);
        public AppResponse Conflict(string m = "", dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Conflict, m, data);

        public AppResponse Failure(IEnumerable<string> m, dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Failure, m, data);
        public AppResponse Validation(IEnumerable<string> m, dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Validation, m, data);
        public AppResponse NotFound(IEnumerable<string> m, dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.NotFound, m, data);
        public AppResponse Unexpected(IEnumerable<string> m, dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Unexpected, m, data);
        public AppResponse Conflict(IEnumerable<string> m, dynamic data = default!) => SetErrorType(ResponseEnums.ErrorType.Conflict, m, data);
    }
}
