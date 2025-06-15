using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a status that an order can have in the system.
    /// </summary>
    public class OrderStatus
    {
        /// <summary>
        /// Unique identifier for the order status.
        /// </summary>
        [Key]
        public int StatusId { get; set; }

        /// <summary>
        /// Descriptive name of the order status (e.g., Pending, Completed, Incomplete).
        /// </summary>
        [StringLength(100)]
        public required string Name { get; set; }

        public ICollection<StockPurchaseOrderItems>? StockPurchaseOrderItems { get; set; }

    }
}
