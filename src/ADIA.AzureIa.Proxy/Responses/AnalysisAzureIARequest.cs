namespace ADIA.AzureIa.Proxy.Responses;

/// <summary>
/// Representa el modelo requerido para analizar un archivo usando Azure IA
/// </summary>
public record AnalysisAzureIARequest
{
    /// <summary>
    /// Representa el archivo en byte[]
    /// </summary>
    public byte[] File { get; set; }
}