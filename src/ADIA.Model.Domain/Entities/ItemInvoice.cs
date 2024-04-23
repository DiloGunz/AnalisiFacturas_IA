using ADIA.Model.Domain.Core;

namespace ADIA.Model.Domain.Entities;

/// <summary>
/// Representa un Item de factura
/// </summary>
public class ItemInvoice : EntityBase
{
    /// <summary>
    /// Representa la descripcion del item
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Representa la cantidad del item
    /// </summary>
    public decimal Quantity { get; set; }
    /// <summary>
    /// Representa el precio unitario
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// Representa el tottal del item
    /// </summary>
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// Representa el ID de la factura
    /// </summary>
    public long IdInvoice { get; set; }
}