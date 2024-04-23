using ADIA.Shared.Response.Enums;
using System.Text.Json.Serialization;

namespace ADIA.Shared.Response
{
    public abstract record AppResponseBase
    {
        [JsonPropertyOrder(0)]
        public bool IsSuccess { get; set; }


        [JsonPropertyOrder(1)]
        public ResponseEnums.ErrorType ErrorType { get; set; }


        [JsonPropertyOrder(2)]
        public string? Function { get; set; }


        [JsonPropertyOrder(3)]
        public string? Href { get; set; }


        [JsonPropertyOrder(4)]
        public IEnumerable<string> Messages { get; private set; } = null!;



        [JsonIgnore]
        public string Message => Messages is not null && Messages.Any() ? Messages.Order().First() : GetDefaultMessage(false);

        protected void PrepareResponse(bool isSuccess, IEnumerable<string> messages = default!)
        {
            IsSuccess = isSuccess;

            if (messages != null && messages.Any())
            {
                Messages = messages.OrderBy(x => x).ToList();
            }
            else
            {
                Messages = new List<string>() { GetDefaultMessage(isSuccess) };
            }
        }

        private static string GetDefaultMessage(bool isSuccess) =>
            isSuccess ? "La operación ha finalizado con éxito" :
                         "Hemos tenido un problema al ejecutar la operación. Esperamos su comprensión.";
    }
}
