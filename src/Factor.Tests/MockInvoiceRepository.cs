using Factor.Application.DTOs;
using Factor.Application.Interfaces;

namespace Factor.Tests;

public class MockInvoiceRepository : IInvoiceRepository
{
    private readonly List<InvoiceDto> _invoices = new();
    private int _currentId = 1;

    public Task<int> CreateAsync(CreateInvoiceDto dto)
    {
        var invoice = new InvoiceDto
        {
            FactorId = _currentId++,
            FactorNo = dto.FactorNo,
            FactorDate = dto.FactorDate,
            Customer = dto.Customer,
            DelivaryType = dto.DelivaryType,
            Items = dto.Items,
            TotalPrice = (long)dto.Items.Sum(item => item.Count * item.UnitPrice)
        };
        _invoices.Add(invoice);
        return Task.FromResult(invoice.FactorId);
    }

    public Task<bool> UpdateAsync(UpdateInvoiceDto dto)
    {
        var invoice = _invoices.FirstOrDefault(i => i.FactorId == dto.FactorId);
        if (invoice == null)
        {
            return Task.FromResult(false);
        }

        invoice.FactorNo = dto.FactorNo;
        invoice.FactorDate = dto.FactorDate;
        invoice.Customer = dto.Customer;
        invoice.DelivaryType = dto.DelivaryType;
        invoice.Items = dto.Items;
        invoice.TotalPrice =(long) dto.Items.Sum(item => item.Count * item.UnitPrice);

        return Task.FromResult(true);
    }

    public Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var invoice = _invoices.FirstOrDefault(i => i.FactorId == id);
        return Task.FromResult(invoice);
    }

    public Task<List<InvoiceDto>> SearchAsync(string? customer, int? factorNo, DateTime? fromDate, DateTime? toDate)
    {
        var query = _invoices.AsQueryable();

        if (!string.IsNullOrEmpty(customer))
        {
            query = query.Where(i => i.Customer == customer);
        }

        if (factorNo.HasValue)
        {
            query = query.Where(i => i.FactorNo == factorNo.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(i => i.FactorDate >= DateOnly.FromDateTime(fromDate.Value));
        }

        if (toDate.HasValue)
        {
            query = query.Where(i => i.FactorDate <=DateOnly.FromDateTime (toDate.Value));
        }

        return Task.FromResult(query.ToList());
    }
}