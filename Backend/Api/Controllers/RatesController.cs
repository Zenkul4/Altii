using Application.Dtos.Rate;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Desktop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatesController : ControllerBase
{
    private readonly IRateService _rateService;

    public RatesController(IRateService rateService)
    {
        _rateService = rateService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _rateService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpGet("season/{seasonId}")]
    public async Task<IActionResult> GetBySeason(int seasonId, CancellationToken ct)
    {
        var result = await _rateService.GetBySeasonAsync(seasonId, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRateDto dto,
        [FromQuery] int createdById,
        CancellationToken ct)
    {
        var result = await _rateService.CreateAsync(dto, createdById, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRateDto dto, CancellationToken ct)
    {
        var result = await _rateService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }
}