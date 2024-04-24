using Newtonsoft.Json.Linq;

namespace ADIA.OpenAi.Proxy.Utils;

/// <summary>
/// Proporciona métodos estáticos para validar y procesar respuestas obtenidas de la API de OpenAI.
/// </summary>
public class ValidateOpenIaResponse
{
    /// <summary>
    /// Procesa el contenido de la respuesta para asegurar que no sea nula, vacía, ni contenga errores específicos como 'BadRequest'.
    /// </summary>
    /// <param name="content">El contenido de la respuesta obtenida de la API de OpenAI.</param>
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

    /// <summary>
    /// Extrae y retorna el mensaje de error de una respuesta 'BadRequest' utilizando el formato JSON contenido en la respuesta.
    /// </summary>
    /// <param name="content">El contenido de la respuesta que incluye un error 'BadRequest'.</param>
    /// <returns>El mensaje de error extraído del contenido de la respuesta.</returns>
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