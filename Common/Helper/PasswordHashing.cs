using InventoryManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.Common.Helper
{
    public class PasswordHashing
    {
        private static readonly PasswordHasher<Users> passwordHasher = new PasswordHasher<Users>();

        /// <summary>
        /// Hash a password using ASP.NET Core Identity
        /// </summary>
        public static string HashPassword(Users user, string password)
        {
            return passwordHasher.HashPassword(user, password);
        }

        /// <summary>
        /// Verify a hashed password
        /// </summary>
        public static bool VerifyPassword(Users user, string hashedPassword, string providedPassword)
        {
            return passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}
