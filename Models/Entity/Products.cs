using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a product in the inventory management system.
    /// </summary>
    public class Products
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        [Key]
        public long ProductId { get; set; }

        /// <summary>
        /// Name of the product (maximum 255 characters).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// Description of the product.
        /// </summary>
        [StringLength(225)]
        public string? Description { get; set; }

        /// <summary>
        /// Category of the product (maximum 100 characters).
        /// </summary>
        [StringLength(100)]
        public int CategoryId { get; set; }

        /// <summary>
        /// Price of the product (decimal with 18 digits, 2 decimal places).
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Identifier of the user who created the product.
        /// </summary>
        /// 
        public int AvailableQuantity { get; set; }
        public int MinimumRequiredQuantity { get; set; }

        public DateTime LastRestockAt { get; set; }
        public long CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the product was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } 

        /// <summary>
        /// Identifier of the user who last updated the product.
        /// </summary>
        public long UpdatedBy { get; set; }

        /// <summary>
        /// Date and time when the product was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } 

        /// <summary>
        /// Indicates whether the product is deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Status of the product.
        /// </summary>
        public int ProductStatus { get; set; }

        public Users? CreatedByUser { get; set; }
        public Users? UpdatedByUser { get; set; }
        public Category? Category { get; set; }

        public ICollection<StockPurchaseOrderItems>? StockPurchaseOrderItems { get; set; }
    }
}