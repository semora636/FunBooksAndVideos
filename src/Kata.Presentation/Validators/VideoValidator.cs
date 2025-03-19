using FluentValidation;
using Kata.Domain.Entities;

namespace Kata.Presentation.Validators
{
    public class VideoValidator : AbstractValidator<Video>
    {
        public VideoValidator()
        {
            RuleFor(video => video.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

            RuleFor(video => video.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");

            RuleFor(video => video.Director)
                .MaximumLength(100).WithMessage("Director name must be at most 100 characters.")
                .When(video => !string.IsNullOrEmpty(video.Director));
        }
    }
}