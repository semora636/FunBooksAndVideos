using FluentValidation;
using Kata.Domain.Entities;

namespace Kata.Presentation.Validators
{
    public class PurchaseOrderValidator : AbstractValidator<PurchaseOrder>
    {
        public PurchaseOrderValidator(OrderItemValidator orderItemValidator)
        {
            RuleFor(order => order.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId must be greater than 0.");

            RuleFor(order => order.Items)
                .Must(items => items?.Count > 0)
                .WithMessage("Items must contain at least one item.");

            RuleForEach(order => order.Items)
                .SetValidator(orderItemValidator)
                .When(order => order.Items != null);
        }
    }
}
