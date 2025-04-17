namespace Factor.Application.DTOs;

// Application/DTOs/InvoiceDetailDto.cs
public class InvoiceDetailDto
{
    public int ProductId { get; set; }
    public string? ProductDescription { get; set; }
    public decimal Count { get; set; }
    public int UnitPrice { get; set; }
    public long SumPrice => (long)(Count * UnitPrice);
}