using ADIA.Model.Domain.Core;

namespace ADIA.Model.Domain.Entities;

/// <summary>
/// Representa una factura
/// </summary>
public class Invoice : EntityBase
{
    /// <summary>
    /// Representa el nombre del proveedor
    /// </summary>
    public string SupplierName { get; set; }
    /// <summary>
    /// Representa la direccion del proveedor
    /// </summary>
    public string SupplierAddress { get; set; }
    /// <summary>
    /// Representa el nombre del cliente
    /// </summary>
    public string CustomerName { get; set; }
    /// <summary>
    /// Representa la direccion del cliente
    /// </summary>
    public string CustomerAddress { get; set; }
    /// <summary>
    /// Representa el numero de la factura
    /// </summary>
    public string InvoiceNumber { get; set; }
    /// <summary>
    /// Representa la fecha de la factura
    /// </summary>
    public string InvoiceDate { get; set; }
    /// <summary>
    /// Representa la moneda de la factura
    /// </summary>
    public string Currency { get; set; }
    /// <summary>
    /// Representa el total de la factura
    /// </summary>
    public decimal TotalAmmount { get; set; }
    /// <summary>
    /// Representa el ID del analisis realizado
    /// </summary>
    public long IdAnalysisResponse { get; set; }
    /// <summary>
    /// Representa el detalle de la factura (Items o Productos asociados)
    /// </summary>
    public List<ItemInvoice> Items { get; set; } 
}