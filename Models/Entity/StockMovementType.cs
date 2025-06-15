using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    /// <summary>
    /// Represents a type of inventory movement, such as inbound, outbound, or transfer.
    /// </summary>
    public class StockMovementType
    {
        /// <summary>
        /// Unique identifier for the movement type.
        /// </summary>
        [Key]
        public int MovementTypeId { get; set; }

        /// <summary>
        /// Name or description of the movement type.
        /// </summary>
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
    }
}
