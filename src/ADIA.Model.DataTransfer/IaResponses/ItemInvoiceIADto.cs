namespace ADIA.Model.DataTransfer.IaResponses;

public record ItemInvoiceIADto
{
    public string Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
}