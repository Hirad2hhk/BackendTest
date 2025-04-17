namespace Factor.Application.DTOs;

// Application/DTOs/InvoiceDto.cs
public class InvoiceDto
{
    public int FactorNo { get; set; }
    public DateOnly FactorDate { get; set; }
    public string? Customer { get; set; }
    public byte? DelivaryType { get; set; }
    public List<InvoiceDetailDto> Details { get; set; } = new();
}