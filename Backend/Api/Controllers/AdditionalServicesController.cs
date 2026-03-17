using Application.Dtos.AdditionalService;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Desktop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdditionalServicesController : ControllerBase
{
    private readonly IAdditionalServiceService _service;

    public AdditionalServicesController(IAdditionalServiceService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActive(CancellationToken ct)
    {
        var result = await _service.GetAllActiveAsync(ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAdditionalServiceDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAdditionalServiceDto dto, CancellationToken ct)
    {
        var result = await _service.UpdateAsync(id, dto, ct);
        return Ok(result);
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        await _service.DeactivateAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        await _service.ActivateAsync(id, ct);
        return NoContent();
    }
}