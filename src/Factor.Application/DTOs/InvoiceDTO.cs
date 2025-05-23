using Factor.Domain;

namespace Factor.Application.DTOs;

// Application/DTOs/InvoiceDto.cs
public class InvoiceDto
{
    public int FactorId { get; set; }
    public int FactorNo { get; set; }
    public DateOnly FactorDate { get; set; }
    public string? Customer { get; set; }
    public DeliveryType? DelivaryType { get; set; }
    public long TotalPrice { get; set; }
    public List<InvoiceItemDto> Items { get; set; } = new();
}