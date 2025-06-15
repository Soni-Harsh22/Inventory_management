using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a single item within an order, including product details and pricing.
    /// </summary>
    public class OrderItems
    {
        /// <summary>
        /// Unique identifier for the order item.
        /// </summary>
        [Key]
        public long OrderItemId { get; set; }

        /// <summary>
        /// Identifier of the order this item belongs to.
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Identifier of the product included in the order.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Quantity of the product ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price of a single unit of the product at the time of order.
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Total price for this order item (ProductPrice × Quantity).
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Current status of the order item (e.g., pending, shipped, delivered).
        /// </summary>
        public int OrderItemStatus { get; set; }

        /// <summary>
        /// Navigation property for the associated order.
        /// </summary>
        public Orders Order { get; set; }

        /// <summary>
        /// Navigation property for the associated product.
        /// </summary>
        public Products Product { get; set; }

        public OrderStatus OrderItemStatusNavigation { get; set; }
    }
}
