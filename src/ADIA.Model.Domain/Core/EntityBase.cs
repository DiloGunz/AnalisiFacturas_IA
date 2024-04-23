using ADIA.Model.Domain.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADIA.Model.Domain.Core;

/// <summary>
/// Represent la entidad base, de la cual heredan todas las demás entidades
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Representa el ID de la entidad
    /// No es autogerado
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; } = IdHelper.NextId();

}