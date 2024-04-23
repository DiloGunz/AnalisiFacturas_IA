namespace ADIA.Model.DataTransfer.Dtos.InvoiceDtos;

public record ItemInvoiceDto
{
    public long Id { get; set; }
    public string Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
}