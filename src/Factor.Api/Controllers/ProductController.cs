using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Factor.Api.Controllers;

// Api/Controllers/ProductController.cs
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repo;

    public ProductController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductDto dto)
    {
        var id = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductDto dto)
    {
        if (id != dto.ProductId) return BadRequest();
        var updated = await _repo.UpdateAsync(dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? code, [FromQuery] string? name)
    {
        return Ok(await _repo.SearchAsync(code, name));
    }
}