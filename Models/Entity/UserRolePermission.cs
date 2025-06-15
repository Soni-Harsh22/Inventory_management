using System.ComponentModel.DataAnnotations;
using System.Security;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a specific permission assigned to a user in the inventory management system.
    /// </summary>
    public class UserRolePermission
    {
        /// <summary>
        /// Unique identifier for the user role permission mapping.
        /// </summary>
        [Key]
        public int UserRolePermissionId { get; set; }

        /// <summary>
        /// Identifier of the associated user.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Identifier of the associated permission.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Indicates whether the permission is allowed for the user.
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// The associated user entity.
        /// </summary>
        public Users User { get; set; }

        /// <summary>
        /// The associated permission entity.
        /// </summary>
        public Permissions Permission { get; set; }
    }
}