using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a mapping between a role and a permission in the inventory management system.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// Unique identifier for the role permission mapping.
        /// </summary>
        [Key]
        public int RolePermissionId { get; set; }

        /// <summary>
        /// Identifier of the associated role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Identifier of the associated permission.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// The associated role entity.
        /// </summary>
        public Roles Role { get; set; }

        /// <summary>
        /// The associated permission entity.
        /// </summary>
        public Permissions Permission { get; set; }
    }
}