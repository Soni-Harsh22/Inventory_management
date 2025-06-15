using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    public class StocksMovement
    {
        [Key]
        public long MovementId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public int MovementTypeId { get; set; }

        public string? Comments { get; set; }

        [Required]
        public int PreviousQuantity { get; set; }

        [Required]
        public int QuantityChange { get; set; }

        [Required]
        public int UpdateQuantity { get; set; }

        [Required]
        public DateTime MovementDate { get; set; } 

        public long UpdateBy { get; set; }

        [ForeignKey("MovementTypeId")]
        public StockMovementType? MovementType { get; set; }

        [ForeignKey("ProductId")]
        public Products? Product { get; set; }

        [ForeignKey(nameof(UpdateBy))] 
        public Users? UpdatedByUser { get; set; }
    }

}