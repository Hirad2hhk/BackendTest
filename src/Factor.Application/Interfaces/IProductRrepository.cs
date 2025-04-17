using Factor.Application.DTOs;

namespace Factor.Application.Interfaces;

public interface IProductRepository
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ProductDto product);
    Task<bool> UpdateAsync(ProductDto product);
    Task<bool> DeleteAsync(int id);
}