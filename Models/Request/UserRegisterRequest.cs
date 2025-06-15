using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Request
{
    /// <summary>
    /// Request model for user registration.
    /// </summary>
    public class UserRegisterRequest
    {
        /// <summary>
        /// Full name of the user.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Email address for the user.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        /// <summary>
        /// Password for the user.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        /// <summary>
        /// Role ID for the user (e.g., Admin, Employee, Customer).
        /// </summary>
        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }
    }
}
