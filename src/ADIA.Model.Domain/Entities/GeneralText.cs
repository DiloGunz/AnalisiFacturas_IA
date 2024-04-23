using ADIA.Model.Domain.Core;

namespace ADIA.Model.Domain.Entities;

/// <summary>
/// Representa un Texto General
/// </summary>
public class GeneralText : EntityBase
{
    /// <summary>
    /// Representa la descripción del documento analizado
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Representa el resumen del documento analizado
    /// </summary>
    public string Summary { get; set; }
    /// <summary>
    /// Representa el sentimiento del documento analizado
    /// </summary>
    public string Sentiment { get; set; }
    /// <summary>
    /// Representa el ID de la respuesta del analisis realizado
    /// </summary>
    public long IdAnalysisResponse { get; set; }
}