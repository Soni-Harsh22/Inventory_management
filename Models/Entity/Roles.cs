using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a role in the inventory management system.
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Unique identifier for the role.
        /// </summary>
        [Key]
        public int RoleId { get; set; }

        /// <summary>
        /// Name of the role.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        ///
        /// </summary>
        public ICollection<Users>? Users { get; set; }

    }
}