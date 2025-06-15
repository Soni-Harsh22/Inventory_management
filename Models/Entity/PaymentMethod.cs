using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a method of payment available in the system, such as credit card, UPI, or wallet.
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Unique identifier for the payment method.
        /// </summary>
        [Key]
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Name or description of the payment method.
        /// </summary>
        [StringLength(100)]
        public required string Name { get; set; }
    }
}
