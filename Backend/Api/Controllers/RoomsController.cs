using Application.DTOs.Room;
using Application.Services.Interfaces;
using Alti.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Desktop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _roomService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable(
        [FromQuery] DateOnly checkIn,
        [FromQuery] DateOnly checkOut,
        [FromQuery] RoomType? type = null,
        [FromQuery] short? minCapacity = null,
        CancellationToken ct = default)
    {
        var result = await _roomService.GetAvailableAsync(checkIn, checkOut, type, minCapacity, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto, CancellationToken ct)
    {
        var result = await _roomService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto, CancellationToken ct)
    {
        var result = await _roomService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }

    [HttpPatch("{id}/disable")]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        await _roomService.DisableAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/enable")]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        await _roomService.EnableAsync(id, ct);
        return NoContent();
    }
}