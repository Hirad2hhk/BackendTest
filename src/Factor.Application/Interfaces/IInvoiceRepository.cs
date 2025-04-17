using Factor.Application.DTOs;

namespace Factor.Application.Interfaces;

public interface IInvoiceRepository
{
    Task<int> CreateAsync(CreateInvoiceDto dto);
    Task<bool> UpdateAsync(UpdateInvoiceDto dto);
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<List<InvoiceDto>> SearchAsync(string? customer, int? factorNo, DateTime? fromDate, DateTime? toDate);
}