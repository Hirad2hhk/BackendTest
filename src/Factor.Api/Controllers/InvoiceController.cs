using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Factor.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRepository _repo;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceDto dto)
    {
        var id = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(Create), new { id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceDto dto)
    {
        if (id != dto.FactorId) return BadRequest();

        var updated = await _repo.UpdateAsync(dto);
        return updated ? NoContent() : NotFound();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _repo.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? customer,
        [FromQuery] int? factorNo,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var results = await _repo.SearchAsync(customer, factorNo, fromDate, toDate);
        return Ok(results);
    }
}
