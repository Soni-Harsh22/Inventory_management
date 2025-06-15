using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class StockPurchaseOrderItemRequestValidator : AbstractValidator<StockPurchaseOrderItemRequest>
    {
        public StockPurchaseOrderItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0).WithMessage("ProductPrice must be greater than 0.");
        }
    }
}
