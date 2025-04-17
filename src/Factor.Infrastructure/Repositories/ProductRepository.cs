using AutoMapper;
using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Factor.Infrastructure.Persistence.Models;
using Factor.Persistence.Entities.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Factor.Infrastructure.Repositories;

// Infrastructure/Repositories/ProductRepository.cs
public class ProductRepository : IProductRepository {
    private readonly FactorDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(FactorDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var entities = await _context.Products.ToListAsync();
        return _mapper.Map<List<ProductDto>>(entities);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        return entity is null ? null : _mapper.Map<ProductDto>(entity);
    }

    public async Task<int> CreateAsync(ProductDto dto)
    {
        var entity = _mapper.Map<Product>(dto);
        entity.ChangeDate = DateTime.Now;
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return entity.ProductId;
    }

    public async Task<bool> UpdateAsync(ProductDto dto)
    {
        var entity = await _context.Products.FindAsync(dto.ProductId);
        if (entity == null) return false;

        _mapper.Map(dto, entity);
        entity.ChangeDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity == null) return false;

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}