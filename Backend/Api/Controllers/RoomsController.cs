using Alti.Domain.Enums;
using Application.DTOs.Room;
using Application.Interfaces;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IRoomAdminService _roomAdminService;

    public RoomsController(IRoomService roomService, IRoomAdminService roomAdminService)
    {
        _roomService = roomService;
        _roomAdminService = roomAdminService;
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
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto, CancellationToken ct)
    {
        var result = await _roomService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto, CancellationToken ct)
    {
        var result = await _roomService.UpdateAsync(id, dto, ct);
        return Ok(result);
    }

    [HttpPatch("{id}/disable")]
    [Authorize]
    public async Task<IActionResult> Disable(int id, CancellationToken ct)
    {
        await _roomService.DisableAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/enable")]
    [Authorize]
    public async Task<IActionResult> Enable(int id, CancellationToken ct)
    {
        await _roomService.EnableAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/mark-available")]
    [Authorize]
    public async Task<IActionResult> MarkAsAvailable(int id, CancellationToken ct)
    {
        await _roomService.MarkAsAvailableAsync(id, ct);
        return NoContent();
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomAdminService.GetAllRoomsAsync();
        return Ok(rooms);
    }

    [HttpPatch("{id}/mark-occupied")]
    [Authorize]
    public async Task<IActionResult> MarkOccupied(int id)
    {
        await _roomAdminService.MarkOccupiedAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/mark-cleaning")]
    [Authorize]
    public async Task<IActionResult> MarkCleaning(int id)
    {
        await _roomAdminService.MarkCleaningAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/mark-blocked")]
    [Authorize]
    public async Task<IActionResult> MarkBlocked(int id)
    {
        await _roomAdminService.MarkBlockedAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/release-block")]
    [Authorize]
    public async Task<IActionResult> ReleaseBlock(int id)
    {
        await _roomAdminService.ReleaseBlockAsync(id);
        return NoContent();
    }
}