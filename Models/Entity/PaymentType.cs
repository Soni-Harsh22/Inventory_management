using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a type of payment method in the inventory management system.
    /// </summary>
    public class PaymentType
    {
        /// <summary>
        /// Unique identifier for the payment type.
        /// </summary>
        [Key]
        public int PaymentTypeId { get; set; }

        /// <summary>
        /// Name of the payment type (e.g., Cash, Credit Card, PayPal).
        /// </summary>
        [StringLength(100)]
        public required string Name { get; set; }
    }
}