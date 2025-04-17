using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Factor.Infrastructure.Persistence.Models;
using Factor.Persistence.Entities.Persistence.Models;

namespace Factor.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly FactorDbContext _context;

    public InvoiceRepository(FactorDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(InvoiceDto dto)
    {
        var factor = new Factor.Persistence.Entities.Persistence.Models.Factor
        {
            FactorNo = dto.FactorNo,
            FactorDate = dto.FactorDate,
            Customer = dto.Customer,
            DelivaryType = dto.DelivaryType,
            TotalPrice = dto.Details.Sum(d => d.SumPrice)
        };

        _context.Factors.Add(factor);
        await _context.SaveChangesAsync(); // get FactorId

        foreach (var detail in dto.Details)
        {
            var entity = new FactorDetail
            {
                FactorId = factor.FactorId,
                ProductId = detail.ProductId,
                ProductDescription = detail.ProductDescription,
                Count = detail.Count,
                UnitPrice = detail.UnitPrice,
                SumPrice = detail.SumPrice
            };
            _context.FactorDetails.Add(entity);
        }

        await _context.SaveChangesAsync();
        return factor.FactorId;
    }
}