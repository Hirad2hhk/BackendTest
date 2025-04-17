using Factor.Application.DTOs;

namespace Factor.Application.Interfaces;

public interface IInvoiceRepository
{
    Task<int> CreateAsync(InvoiceDto dto);
    // Optionally add Get, Update, Delete
}