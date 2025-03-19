using FluentValidation;
using Kata.Domain.Entities;

namespace Kata.Presentation.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

            RuleFor(book => book.Author)
                .MaximumLength(100).WithMessage("Author must be at most 100 characters.")
                .When(book => !string.IsNullOrEmpty(book.Author));

            RuleFor(book => book.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}