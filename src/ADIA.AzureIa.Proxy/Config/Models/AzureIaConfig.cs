namespace ADIA.AzureIa.Proxy.Config.Models;

/// <summary>
/// Archivo que representa la configuracion para esta libreria de clases
/// </summary>
public record AzureIaConfig
{
    /// <summary>
    /// Representa el Api Key de azure IA
    /// </summary>
    public string ApiKey { get; set; }
    /// <summary>
    /// Representa el end point de azure IA
    /// </summary>
    public string EndPoint { get; set; }
}