using Application.Dtos.Payment;
using FluentValidation;

namespace Application.Validators;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("Booking ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(50).When(x => x.PaymentMethod is not null);
    }
}