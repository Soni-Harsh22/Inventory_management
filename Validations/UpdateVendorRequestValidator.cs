using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class UpdateVendorRequestValidator : AbstractValidator<UpdateVendorRequest>
    {
        public UpdateVendorRequestValidator()
        {
            RuleFor(x => x.VendorId)
                .GreaterThan(0).WithMessage("Vendor ID must be a positive number.");

            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);
            });

            When(x => x.StoreName != null, () =>
            {
                RuleFor(x => x.StoreName)
                    .NotEmpty()
                    .MaximumLength(100);
            });

            When(x => x.Phone != null, () =>
            {
                RuleFor(x => x.Phone)
                    .Matches(@"^[0-9]{10,15}$")
                    .WithMessage("Phone must be 10 to 15 digits.");
            });

            When(x => x.Email != null, () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Invalid email format.");
            });

            When(x => x.Pincode != null, () =>
            {
                RuleFor(x => x.Pincode)
                    .Matches(@"^\d{5,10}$").WithMessage("Pincode must be 5 to 10 digits.");
            });
        }
    }
}