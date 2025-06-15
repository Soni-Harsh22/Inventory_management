using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a product category in the inventory system.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// ID of the user who created the category.
        /// </summary>
        public long CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the category was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } 

        /// <summary>
        /// ID of the user who last updated the category.
        /// </summary>
        public long UpdatedBy { get; set; }

        /// <summary>
        /// Date and time when the category was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } 

        /// <summary>
        /// Indicates whether the category is currently active.
        /// </summary>
        public bool ActiveStatus { get; set; } 

        /// <summary>
        /// Indicates whether the category is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; } 

        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        public Users CreatedByUser { get; set; }

        //[ForeignKey("UpdatedBy")]
        //public Users UpdatedByUser { get; set; }

    }
}
