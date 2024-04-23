using Newtonsoft.Json.Linq;

namespace ADIA.OpenAi.Proxy.Utils;

public class ValidateOpenIaResponse
{
    public static void Process(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new Exception("Respuesta nula o vacía.");
        }

        if (content.Contains("BadRequest"))
        {
            throw new Exception(GetMessageOfBadRequestResponse(content));
        }
    }

    public static string GetMessageOfBadRequestResponse(string content)
    {
        if (content.Contains("BadRequest"))
        {
            int startIndex = content.IndexOf('{');
            int endIndex = content.LastIndexOf('}');
            string jsonString = content.Substring(startIndex, endIndex - startIndex + 1);
            JObject jObject = JObject.Parse(jsonString);
            return jObject["error"]!["message"]!.ToString();
        }

        return content;
    }
}