using Application.Dtos.Payment;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _paymentService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    [HttpGet("booking/{bookingId}")]
    public async Task<IActionResult> GetByBooking(int bookingId, CancellationToken ct)
    {
        var result = await _paymentService.GetByBookingAsync(bookingId, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto, CancellationToken ct)
    {
        var result = await _paymentService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> Approve(int id, [FromQuery] string externalReference, CancellationToken ct)
    {
        var result = await _paymentService.ApproveAsync(id, externalReference, ct);
        return Ok(result);
    }

    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
    {
        var result = await _paymentService.RejectAsync(id, ct);
        return Ok(result);
    }

    [HttpPatch("{id}/refund")]
    public async Task<IActionResult> Refund(int id, CancellationToken ct)
    {
        var result = await _paymentService.RefundAsync(id, ct);
        return Ok(result);
    }

    [HttpPost("cash/{bookingId}")]
    public async Task<IActionResult> RegisterCash(int bookingId, CancellationToken ct)
    {
        var result = await _paymentService.RegisterCashPaymentAsync(bookingId, ct);
        return Ok(result);
    }
}