using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Models.Entity
{
    public class StockPurchaseOrderItems
    {
        [Key]
        public int StockPurchaseOrderItemId { get; set; }

        // Foreign key for StockPurchaseOrder
        public long StockPurchaseOrderId { get; set; }

        // Foreign key for Product
        public long ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        // Foreign key for OrderStatus
        public int OrderItemStatus { get; set; }

        public DateTime OrderRecivedDate { get; set; }

        // Navigation properties
        public StockPurchaseOrders? StockPurchaseOrder { get; set; }
        public Products? Product { get; set; }
        public OrderStatus? OrderStatus { get; set; }
    }
}