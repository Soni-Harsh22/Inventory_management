using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class CustomerOrderRequestValidator : AbstractValidator<CustomerOrderRequest>
    {
        public CustomerOrderRequestValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Order items are required.")
                .NotEmpty().WithMessage("Order must contain at least one item.");

            RuleForEach(x => x.Items).SetValidator(new CustomerOrderItemRequestValidator());

            RuleFor(x => x.PaymentMethod)
                 .InclusiveBetween(0, 5).WithMessage("Invalid Payment method.");
        }
    }
}
