using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a permission in the inventory management system.
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// Unique identifier for the permission.
        /// </summary>
        [Key]
        public int PermissionId { get; set; }

        /// <summary>
        /// Name of the permission.
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Collection of role permissions associated with this permission.
        /// </summary>
        public ICollection<RolePermission> RolePermissions { get; set; }

        /// <summary>
        /// Collection of user role permissions associated with this permission.
        /// </summary>
        public ICollection<UserRolePermission> UserRolePermissions { get; set; }
    }
}