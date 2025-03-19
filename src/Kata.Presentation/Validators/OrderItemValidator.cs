using FluentValidation;
using Kata.Domain.Entities;

namespace Kata.Presentation.Validators
{
    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {
            RuleFor(item => item.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(item => item.ProductType)
                .IsInEnum().WithMessage("Invalid ProductType.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(item => item.Price)
                .GreaterThan(0).WithMessage("Price must be greater than or equal to 0.");
        }
    }
}