using ADIA.Model.Domain.Core;
using ADIA.Shared.Enums;

namespace ADIA.Model.Domain.Entities;

/// <summary>
/// Representa la respuesta que se obtiene para cada analisis
/// </summary>
public class AnalysisResponse : EntityBase
{
    /// <summary>
    /// Representa la fecha de inicio del analisis
    /// </summary>
    public DateTime StartAnalysis { get; set; }
    /// <summary>
    /// Representa la fecha de fin del analisis
    /// </summary>
    public DateTime EndAnalysis { get; set; }
    /// <summary>
    /// Representa si el analisis se ha efectuado con éxito
    /// No refleja si la IA ha encontrado un documento válido
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// Representa el tiempo que demora la respuesta desde la IA
    /// </summary>
    public decimal ResponseTime { get; set; }
    /// <summary>
    /// Representa el mensjae de respuesta en caso de que la respuesta no sea correcta
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// Representa el tipo de documento devuelto
    /// </summary>
    public EntityEnums.DocumentType DocumentType { get; set; }
    /// <summary>
    /// Representa la IA utilizada
    /// </summary>
    public EntityEnums.Ia Ia { get; set; }
    /// <summary>
    /// Representa la relacion con Análisis
    /// </summary>
    public Analysis Analysis { get; set; }
    /// <summary>
    /// Representa el ID de aálisis relacionado
    /// </summary>
    public long IdAnalysis { get; set; }
    /// <summary>
    /// Representa la relacion con FACTURA, en caso de que el analisis haya determinado que el archivo es una Factura
    /// </summary>
    public Invoice Invoice { get; set; }
    /// <summary>
    /// Representa la relacion con GENERAL TEXT, en caso de que el analisis haya determinado que el archivo es un Texto General
    /// </summary>
    public GeneralText GeneralText { get; set; }
}