using Factor.Domain;

namespace Factor.Application.DTOs;

public class UpdateInvoiceDto
{
    public int FactorId { get; set; }
    public int FactorNo { get; set; }
    public DateOnly FactorDate { get; set; }
    public string? Customer { get; set; }
    public DeliveryType? DelivaryType { get; set; }
    public List<InvoiceItemDto> Items { get; set; } = new();
}