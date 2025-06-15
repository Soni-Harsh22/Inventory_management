using FluentValidation;
using InventoryManagementSystem.Models.Request;

namespace InventoryManagementSystem.Validations
{
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be 100 characters or less");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            //RuleFor(x => x.Password)
            //   .NotEmpty().WithMessage("Password is required.")
            //   .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            //   .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            //   .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            //   .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            //   .Matches("[@$!%*?&#]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required");
               
        }
    }
}
