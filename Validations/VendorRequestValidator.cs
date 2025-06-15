using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class VendorRequestValidator : AbstractValidator<VendorRequest>
    {
        public VendorRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vendor name is required.")
                .MaximumLength(100).WithMessage("Vendor name must not exceed 100 characters.");

            RuleFor(x => x.StoreName)
                .NotEmpty().WithMessage("Store name is required.")
                .MaximumLength(100).WithMessage("Store name must not exceed 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9]{10,15}$").WithMessage("Phone must be 10 to 15 digits.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.");

            RuleFor(x => x.Pincode)
                .NotEmpty().WithMessage("Pincode is required.")
                .Matches(@"^\d{5,10}$").WithMessage("Pincode must be 5 to 10 digits.");
        }
    }
}