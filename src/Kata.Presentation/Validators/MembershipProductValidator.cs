using FluentValidation;
using Kata.Domain.Entities;

namespace Kata.Presentation.Validators
{
    public class MembershipProductValidator : AbstractValidator<MembershipProduct>
    {
        public MembershipProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

            RuleFor(product => product.MembershipType)
                .IsInEnum().WithMessage("Invalid Membership Type.");

            RuleFor(product => product.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleFor(product => product.DurationMonths)
                .GreaterThan(0).WithMessage("Duration Months must be greater than 0.");
        }
    }
}