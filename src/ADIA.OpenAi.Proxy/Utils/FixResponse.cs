namespace ADIA.OpenAi.Proxy.Utils;

/// <summary>
/// Proporciona un método estático para procesar y posiblemente modificar texto que incluya marcadores de formato específicos.
/// </summary>
public class FixResponse
{
    /// <summary>
    /// Procesa el texto de entrada para eliminar marcadores de formato JSON si están presentes al inicio del texto.
    /// Si el texto es muy corto (10 caracteres o menos), lo devuelve sin cambios.
    /// </summary>
    /// <param name="text">El texto que necesita ser procesado.</param>
    /// <returns>El texto procesado, con o sin modificaciones dependiendo de su contenido inicial.</returns>
    public static string Process(string text)
    {
		try
		{
			if (text.Length <= 10)
			{
				return text;
			}

			var stringInicio = text.Substring(0, 10);

			if (stringInicio.ToLower().Contains("json"))
			{
				//var nuevoTexto = text.Substring(7, text.Length - 1 - 3);
				var nuevoTexto = text.Replace("```json", "").Replace("```", "");
				return nuevoTexto;
			}
		}
		catch
		{
			
		}

        return text;
    }
}