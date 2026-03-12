using Application.Dtos.Room;
using Application.DTOs.Room;
using FluentValidation;

namespace Application.Validators;

public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Room number is required.")
            .MaximumLength(10);

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than zero.");

        RuleFor(x => x.Capacity)
            .InclusiveBetween((short)1, (short)20)
            .WithMessage("Capacity must be between 1 and 20.");

        RuleFor(x => x.Floor)
            .GreaterThanOrEqualTo((short)0)
            .WithMessage("Floor cannot be negative.");
    }
}