using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Request
{
    /// <summary>
    /// Request model for user login.
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// Email address of the user.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}