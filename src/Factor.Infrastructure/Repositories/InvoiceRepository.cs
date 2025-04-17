using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Factor.Persistence.Entities.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Factor.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly FactorContext _context;

    public InvoiceRepository(FactorContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(CreateInvoiceDto dto)
    {
        var factor = new Factor.Persistence.Entities.Persistence.Models.Factor
        {
            FactorNo = dto.FactorNo,
            FactorDate = dto.FactorDate,
            Customer = dto.Customer,
            DelivaryType = dto.DelivaryType,
            TotalPrice = dto.Items.Sum(i => (long)(i.Count * i.UnitPrice))
        };

        _context.Factors.Add(factor);
        await _context.SaveChangesAsync();

        foreach (var item in dto.Items)
        {
            _context.FactorDetails.Add(new FactorDetail
            {
                FactorId = factor.FactorId,
                ProductId = item.ProductId,
                ProductDescription = item.ProductDescription,
                Count = item.Count,
                UnitPrice = item.UnitPrice,
                SumPrice = (long)(item.Count * item.UnitPrice)
            });
        }

        await _context.SaveChangesAsync();
        return factor.FactorId;
    }
    public async Task<bool> UpdateAsync(UpdateInvoiceDto dto)
    {
        var factor = await _context.Factors
            .Include(f => f.FactorDetails)
            .FirstOrDefaultAsync(f => f.FactorId == dto.FactorId);

        if (factor == null) return false;

        // Update main info
        factor.FactorNo = dto.FactorNo;
        factor.FactorDate = dto.FactorDate;
        factor.Customer = dto.Customer;
        factor.DelivaryType = dto.DelivaryType;

        // Remove old items
        _context.FactorDetails.RemoveRange(factor.FactorDetails);

        // Add new items
        var details = dto.Items.Select(i => new FactorDetail
        {
            FactorId = factor.FactorId,
            ProductId = i.ProductId,
            ProductDescription = i.ProductDescription,
            Count = i.Count,
            UnitPrice = i.UnitPrice,
            SumPrice = (long)(i.Count * i.UnitPrice)
        }).ToList();

        factor.TotalPrice = details.Sum(d => d.SumPrice);
        await _context.FactorDetails.AddRangeAsync(details);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var factor = await _context.Factors
            .Include(f => f.FactorDetails)
            .FirstOrDefaultAsync(f => f.FactorId == id);

        if (factor == null) return null;

        return new InvoiceDto
        {
            FactorNo = factor.FactorNo,
            FactorDate = factor.FactorDate,
            Customer = factor.Customer,
            DelivaryType = factor.DelivaryType,
            TotalPrice = factor.TotalPrice ?? 0,
            Items = factor.FactorDetails.Select(d => new InvoiceItemDto
            {
                ProductId = d.ProductId,
                ProductDescription = d.ProductDescription,
                Count = d.Count,
                UnitPrice = d.UnitPrice
            }).ToList()
        };
    }
    public async Task<List<InvoiceDto>> SearchAsync(string? customer, int? factorNo, DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.Factors.Include(f => f.FactorDetails).AsQueryable();

        if (!string.IsNullOrWhiteSpace(customer))
            query = query.Where(f => f.Customer!.Contains(customer));

        if (factorNo.HasValue)
            query = query.Where(f => f.FactorNo == factorNo.Value);

        if (fromDate.HasValue)
            query = query.Where(f => f.FactorDate >= DateOnly.FromDateTime(fromDate.Value));

        if (toDate.HasValue)
            query = query.Where(f => f.FactorDate <= DateOnly.FromDateTime(toDate.Value));

        var results = await query.ToListAsync();

        return results.Select(f => new InvoiceDto
        {
            FactorId = f.FactorId,
            FactorNo = f.FactorNo,
            FactorDate = f.FactorDate,
            Customer = f.Customer,
            DelivaryType = f.DelivaryType,
            TotalPrice = f.TotalPrice ?? 0,
            Items = f.FactorDetails.Select(d => new InvoiceItemDto
            {
                ProductId = d.ProductId,
                ProductDescription = d.ProductDescription,
                Count = d.Count,
                UnitPrice = d.UnitPrice
            }).ToList()
        }).ToList();
    }
}