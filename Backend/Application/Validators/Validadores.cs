using Application.DTOs.Booking;
using Application.DTOs.Payment;
using Application.DTOs.Room;
using Application.DTOs.Season;
using Application.DTOs.User;
using FluentValidation;

namespace Application.Validators;

public sealed class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.Phone)
            .MaximumLength(25)
            .When(x => x.Phone is not null);
    }
}

public sealed class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Room number is required.")
            .MaximumLength(10);

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than zero.");

        RuleFor(x => x.Capacity)
            .InclusiveBetween((short)1, (short)20).WithMessage("Capacity must be between 1 and 20.");

        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo((short)0).WithMessage("Floor cannot be negative.");
    }
}

public sealed class CreateBookingValidator : AbstractValidator<CreateBookingDto>
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

        RuleFor(x => x)
            .Must(x => x.CheckOutDate.DayNumber - x.CheckInDate.DayNumber <= 30)
            .WithMessage("A booking cannot exceed 30 nights.")
            .WithName("Dates");
    }
}

public sealed class CreateSeasonValidator : AbstractValidator<CreateSeasonDto>
{
    public CreateSeasonValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Season name is required.")
            .MaximumLength(100);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");

        RuleFor(x => x.Multiplier)
            .GreaterThan(0).WithMessage("Multiplier must be greater than zero.")
            .LessThanOrEqualTo(5.0m).WithMessage("Multiplier cannot exceed 5.0.");
    }
}

public sealed class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("Booking ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.")
            .LessThanOrEqualTo(99_999.99m).WithMessage("Amount exceeds the allowed limit.");

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(50)
            .When(x => x.PaymentMethod is not null);
    }
}