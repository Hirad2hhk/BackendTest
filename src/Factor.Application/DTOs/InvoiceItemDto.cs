namespace Factor.Application.DTOs;

public class InvoiceItemDto
{
    public int ProductId { get; set; }
    public string? ProductDescription { get; set; }
    public decimal Count { get; set; }
    public int UnitPrice { get; set; }
}