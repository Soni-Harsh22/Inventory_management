using FluentValidation;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class StockPurchaseOrderPartialReceiveRequestValidator : AbstractValidator<StockPurchaseOrderPartialReceiveRequest>
    {
        public StockPurchaseOrderPartialReceiveRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.Status)
                .InclusiveBetween(1, 7).WithMessage("Invalid Status");

            RuleFor(x => x.ProductIds)
                .Must(productIds => productIds == null || productIds.Any())
                .WithMessage("PRODUCT IDS LIST MUST CONTAIN AT LEAST ONE ID.")
                .When(x => x.Status == (int)OrderStatusEnum.Incomplete && x.ProductIds != null)
                .DependentRules(() =>
                {
                    RuleForEach(x => x.ProductIds)
                        .GreaterThan(0)
                        .WithMessage("EACH PRODUCT ID MUST BE GREATER THAN ZERO.");
                });
        }
    }
}