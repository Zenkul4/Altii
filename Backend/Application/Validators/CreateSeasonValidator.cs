using Application.Dtos.Season;
using Application.DTOs.Season;
using FluentValidation;

namespace Application.Validators;

public class CreateSeasonValidator : AbstractValidator<CreateSeasonDto>
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
            .GreaterThan(0).WithMessage("Multiplier must be greater than zero.");
    }
}