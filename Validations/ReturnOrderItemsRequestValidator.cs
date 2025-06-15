using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class ReturnOrderItemsRequestValidator : AbstractValidator<ReturnOrderItemsRequest>
    {
        public ReturnOrderItemsRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.ProductIds)
                .NotEmpty().WithMessage("At least one product ID must be specified.");
        }
    }
}
