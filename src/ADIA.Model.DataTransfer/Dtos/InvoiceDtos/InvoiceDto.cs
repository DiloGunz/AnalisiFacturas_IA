namespace ADIA.Model.DataTransfer.Dtos.InvoiceDtos;

public record InvoiceDto
{
    public InvoiceDto()
    {
        Items = new List<ItemInvoiceDto>();
    }

    public long Id { get; set; }
    public string SupplierName { get; set; }
    public string SupplierAddress { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string InvoiceNumber { get; set; }
    public string InvoiceDate { get; set; }
    public string Currency { get; set; }
    public decimal TotalAmmount { get; set; }

    public List<ItemInvoiceDto> Items { get; set; }

}