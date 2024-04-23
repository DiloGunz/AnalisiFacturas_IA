using ADIA.Model.Domain.Core;
using ADIA.Shared.Enums;

namespace ADIA.Model.Domain.Entities;

/// <summary>
/// Guardar todos los analisis que se hace
/// </summary>
public class Analysis : EntityBase
{
    /// <summary>
    /// representa la fechad de creacion del analisis
    /// </summary>
    public DateTime AnalysisDate { get; set; }
    /// <summary>
    /// Representa el nombre del archivo que se ha subido para analisar
    /// </summary>
    public string FileName { get; set; } = null!;
    /// <summary>
    /// Representa el archivo convertido a BASE64
    /// </summary>
    public string? FileBase64 { get; set; }
    /// <summary>
    /// Representa la extensión del archivo subido
    /// </summary>
    public string FileExtension { get; set; } = null!;
    /// <summary>
    /// Representa el tipo de archivo subido (PDF o omagen)
    /// </summary>
    public EntityEnums.FileType FileType { get; set; }
    /// <summary>
    /// Representa propiedad de navegación hacia la respuesta del analisis
    /// </summary>
    public AnalysisResponse AnalysisResponse { get; set; }

}