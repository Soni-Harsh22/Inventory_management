using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents the status of a payment, such as successful, failed, or pending.
    /// </summary>
    public class PaymentStatus
    {
        /// <summary>
        /// Unique identifier for the payment status.
        /// </summary>
        [Key]
        public int PaymentStatusId { get; set; }

        /// <summary>
        /// Descriptive name of the payment status.
        /// </summary>
        [StringLength(100)]
        public required string Name { get; set; }
    }
}
