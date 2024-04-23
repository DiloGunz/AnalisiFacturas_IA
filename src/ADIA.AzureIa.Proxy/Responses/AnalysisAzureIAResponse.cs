namespace ADIA.OpenAi.Proxy.Responses;

/// <summary>
/// Representa el objeto devuelto por azure IA
/// </summary>
public record AnalysisAzureIAResponse
{
    /// <summary>
    /// Representa el Azure IA ha analizado con éxito el archivo
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Representa la fecha y hora de inicio analisis
    /// </summary>
    public DateTime Start { get; set; }
    /// <summary>
    /// Representa la fecha y hora de fin del analisis
    /// </summary>
    public DateTime End { get; set; }
    /// <summary>
    /// Representa el contenido de la respuesta
    /// </summary>
    public string Result { get; set; }
}