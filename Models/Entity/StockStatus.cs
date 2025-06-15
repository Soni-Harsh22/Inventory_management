using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    public class StockStatus
    {
        [Key]
        public int StockStatusId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
