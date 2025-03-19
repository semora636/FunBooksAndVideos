using FluentValidation;
using Kata.Domain.Entities;

namespace Kata.Presentation.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.FirstName)
                .MaximumLength(100).WithMessage("First Name must be at most 100 characters.");

            RuleFor(customer => customer.LastName)
                .MaximumLength(100).WithMessage("Last Name must be at most 100 characters.");

            RuleFor(customer => customer.EmailAddress)
                .NotEmpty().WithMessage("Email Address is required.")
                .EmailAddress().WithMessage("Email Address is not valid.")
                .MaximumLength(255).WithMessage("Email Address must be at most 255 characters.");

            RuleFor(customer => customer.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(255).WithMessage("Address must be at most 255 characters.");
        }
    }
}