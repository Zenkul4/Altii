using Application.DTOs.BookingService;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Desktop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingServicesController : ControllerBase
{
    private readonly IBookingServiceService _service;

    public BookingServicesController(IBookingServiceService service)
    {
        _service = service;
    }

    [HttpGet("booking/{bookingId}")]
    public async Task<IActionResult> GetByBooking(int bookingId, CancellationToken ct)
    {
        var result = await _service.GetByBookingAsync(bookingId, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBookingServiceDto dto, CancellationToken ct)
    {
        var result = await _service.AddAsync(dto, ct);
        return Ok(result);
    }
}