using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class StockPurchaseOrderRequestValidator : AbstractValidator<StockPurchaseOrderRequest>
    {
        public StockPurchaseOrderRequestValidator()
        {
            RuleFor(x => x.VendorId)
                .GreaterThan(0).WithMessage("VendorId must be greater than 0.");

            RuleFor(x => x.PaymentTypeId)
                .InclusiveBetween(1, 2).WithMessage("Invalid PaymentTypeId");

            RuleFor(x => x.PaymentMethodId)
                .InclusiveBetween(1, 5).WithMessage("Invalid PaymentMethodId");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items list must not be null.")
                .NotEmpty().WithMessage("Items list must not be empty.");

            RuleForEach(x => x.Items).SetValidator(new StockPurchaseOrderItemRequestValidator());
        }
    }
}