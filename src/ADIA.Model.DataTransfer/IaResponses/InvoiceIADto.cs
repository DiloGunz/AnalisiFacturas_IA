namespace ADIA.Model.DataTransfer.IaResponses;

public record InvoiceIADto
{ 
    public string SupplierName { get; set; }
    public string SupplierAddress { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string InvoiceNumber { get; set; }
    public string InvoiceDateTime { get; set; }
    public string Currency { get; set; }
    public decimal TotalAmmount { get; set; }

    public List<ItemInvoiceIADto> Items { get; set; }

    public static InvoiceIADto CreateDefault() => new InvoiceIADto()
    {
        Currency = string.Empty,
        CustomerAddress = string.Empty,
        CustomerName = string.Empty,
        InvoiceDateTime = string.Empty,
        InvoiceNumber = string.Empty,
        SupplierAddress = string.Empty,
        SupplierName = string.Empty,
        TotalAmmount = 0,
        Items = new List<ItemInvoiceIADto>()
        {
            new ItemInvoiceIADto()
            {
                Description = string.Empty,
                Quantity = 0,
                TotalAmount = 0,
                UnitPrice = 0
            }
        }
    };
}