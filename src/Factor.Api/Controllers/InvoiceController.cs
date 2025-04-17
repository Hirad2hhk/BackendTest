using Factor.Application.DTOs;
using Factor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Factor.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceRepository _repo;

    public InvoiceController(IInvoiceRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InvoiceDto dto)
    {
        var id = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(Create), new { id }, null);
    }
}