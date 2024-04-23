namespace ADIA.OpenAi.Proxy.Utils;

public class FixResponse
{
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