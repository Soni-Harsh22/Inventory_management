using System.ComponentModel.DataAnnotations;
using System.Net;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a customer order containing product items, payment details, and delivery address.
    /// </summary>
    public class Orders
    {
        /// <summary>
        /// Unique identifier for the order.
        /// </summary>
        [Key]
        public long OrderId { get; set; }

        /// <summary>
        /// Identifier of the user who placed the order.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Total number of distinct items in the order.
        /// </summary>
        public int NumberOfItems { get; set; }

        /// <summary>
        /// Total quantity of all items combined in the order.
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Current status of the order (e.g., pending, confirmed, shipped).
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// Type of payment used (e.g., online, COD).
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// Method of payment used (e.g., card, UPI, wallet).
        /// </summary>
        public int PaymentMethod { get; set; }

        /// <summary>
        /// Status of the payment (e.g., success, failed, pending).
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// Date and time when the payment was made.
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Total cost of the order including all items and taxes.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// ID of the user or system that last updated the order.
        /// </summary>
        public long UpdatedBy { get; set; }

        /// <summary>
        /// Date and time when the order was last updated.
        /// </summary>
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Navigation property for the user who placed the order.
        /// </summary>
        public Users User { get; set; }

        /// <summary>
        /// Collection of order items included in this order.
        /// </summary>
        public ICollection<OrderItems> OrderItems { get; set; }

        public Users UpdatedByUser { get; set; }
        public OrderStatus OrderStatusNavigation { get; set; }
        public PaymentType PaymentTypeNavigation { get; set; }
        public PaymentMethod PaymentMethodNavigation { get; set; }
        public PaymentStatus PaymentStatusNavigation { get; set; }
    }
}
