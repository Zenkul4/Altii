using Application.DTOs.Booking;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _bookingService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
    {
        var result = await _bookingService.GetByCodeAsync(code, ct);
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _bookingService.GetActiveAsync(page, pageSize, ct);
        return Ok(result);
    }

    [HttpGet("guest/{guestId}")]
    public async Task<IActionResult> GetByGuest(
        int guestId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _bookingService.GetByGuestAsync(guestId, page, pageSize, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingDto dto, CancellationToken ct)
    {
        var result = await _bookingService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id}/confirm")]
    public async Task<IActionResult> Confirm(int id, [FromQuery] int paymentId, CancellationToken ct)
    {
        await _bookingService.ConfirmAsync(id, paymentId, ct);
        return NoContent();
    }

    [HttpPatch("{id}/checkin")]
    public async Task<IActionResult> CheckIn(int id, [FromQuery] int receptionistId, CancellationToken ct)
    {
        await _bookingService.CheckInAsync(id, receptionistId, ct);
        return NoContent();
    }

    [HttpPatch("{id}/checkout")]
    public async Task<IActionResult> CheckOut(int id, CancellationToken ct)
    {
        await _bookingService.CheckOutAsync(id, ct);
        return NoContent();
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        await _bookingService.CancelAsync(id, ct);
        return NoContent();
    }
}