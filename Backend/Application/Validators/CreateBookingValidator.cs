using Application.DTOs.Booking;
using FluentValidation;

namespace Application.Validators;

public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.GuestId)
            .GreaterThan(0).WithMessage("Guest ID is required.");

        RuleFor(x => x.RoomId)
            .GreaterThan(0).WithMessage("Room ID is required.");

        RuleFor(x => x.CheckInDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Check-in date cannot be in the past.");

        RuleFor(x => x.CheckOutDate)
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after check-in date.");
    }
}