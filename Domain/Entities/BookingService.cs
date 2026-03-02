using Alti.Domain.Common;
using System;
using System.Collections.Generic;
namespace Alti.Domain.Entities;

public class BookingService
{
    private BookingService() { }

    public int Id { get; internal set; }
    public int BookingId { get; internal set; }
    public int ServiceId { get; internal set; }
    public int RegisteredById { get; internal set; }
    public short Quantity { get; internal set; }
    public decimal UnitPrice { get; internal set; }
    public DateTimeOffset RegisteredAt { get; internal set; } = DateTimeOffset.UtcNow;
}
