using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a user in the inventory management system.
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        [Key]
        public long UserId { get; set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        /// <summary>
        /// Identifier of the associated role.
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// Phone number of the user.
        /// </summary>
        [Required]
        [StringLength(15)]
        public required string Phone { get; set; }

        /// <summary>
        /// Email address of the user.
        /// </summary>
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Password of the user.
        /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public required string Password { get; set; }

        /// <summary>
        /// Identifier of the user who created this user record.
        /// </summary>
        [Required]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the user record was created.
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } 

        /// <summary>
        /// Identifier of the user who last updated this user record.
        /// </summary>
        [Required]
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Date and time when the user record was last updated.
        /// </summary>
        [Required]
        public DateTime UpdatedDate { get; set; } 

        /// <summary>
        /// Indicates whether the user record is deleted.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
    }
}