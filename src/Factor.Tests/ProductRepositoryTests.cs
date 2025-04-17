
using AutoMapper;
using Factor.Application.DTOs;
using Factor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Factor.Persistence.Entities.Persistence.Models;

namespace Factor.Tests;

public class ProductRepositoryTests
{
    private readonly IMapper _mapper;
    private readonly ProductRepository _repo;

    public ProductRepositoryTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>().ReverseMap();
            cfg.CreateMap<CreateProductDto, Product>();
        });
        _mapper = config.CreateMapper();

        // Create a clean DbContext with only InMemory provider
        var options = new DbContextOptionsBuilder<FactorContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new FactorContext(options);

        // Clear any existing data to ensure test isolation
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Products.Add(new Product
        {
            ProductId = 1,
            ProductCode = "PRD-01",
            ProductName = "Test Product",
            Unit = "pcs",
            ChangeDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
        });
        context.SaveChanges();

        _repo = new ProductRepository(context, _mapper);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllProducts()
    {
        var result = await _repo.GetAllAsync();
        Assert.Single(result);
        Assert.Equal("PRD-01", result.First().ProductCode);
    }

    [Fact]
    public async Task GetById_ShouldReturnCorrectProduct()
    {
        var result = await _repo.GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Test Product", result?.ProductName);
    }

    [Fact]
    public async Task Create_ShouldAddNewProduct()
    {
        var dto = new CreateProductDto
        {
            ProductCode = "PRD-99",
            ProductName = "New Product",
            Unit = "box"
        };

        var id = await _repo.CreateAsync(dto);
        var result = await _repo.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal("New Product", result?.ProductName);
    }

    [Fact]
    public async Task Update_ShouldModifyProduct()
    {
        var updateDto = new ProductDto
        {
            ProductId = 1,
            ProductCode = "PRD-01",
            ProductName = "Updated",
            Unit = "pcs",
            ChangeDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
        };

        var updated = await _repo.UpdateAsync(updateDto);
        var result = await _repo.GetByIdAsync(1);

        Assert.True(updated);
        Assert.Equal("Updated", result?.ProductName);
    }

    [Fact]
    public async Task Delete_ShouldRemoveProduct()
    {
        var deleted = await _repo.DeleteAsync(1);
        var result = await _repo.GetByIdAsync(1);

        Assert.True(deleted);
        Assert.Null(result);
    }
}