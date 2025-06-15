using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Entity
{
    public class StockPurchaseOrders
    {
        [Key]
        public long StockPurchaseOrderId { get; set; }
        public int NumberOfItem { get; set; }
        public int TotalNumberOfQuantity { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public long PurchaseBy { get; set; }
       
        [Required]
        public long VendorId { get; set; }
        
        public int OrderStatusId { get; set; }
        
        public DateTime OrderRecivedDate { get; set; }
        
        public int PaymentTypeId { get; set; }
       
        public int PaymentMethodId { get; set; }
        
        public int PaymentStatusId { get; set; }
        
        public DateTime PaymentDate { get; set; }
        
        public long UpdatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; } 

        [ForeignKey("UpdatedBy")]
        public Users? UpdatedByUser { get; set; }

        [ForeignKey("VendorId")]
        public VendorDetails? VendorDetails { get; set; }

        [ForeignKey("PaymentStatusId")]
        public PaymentStatus? PaymentStatus { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethod? PaymentMethod { get; set; }

        [ForeignKey("PaymentTypeId")]
        public PaymentType? PaymentType { get; set; }

        [ForeignKey("OrderStatusId")]
        public OrderStatus? OrderStatus { get; set; }
        public ICollection<StockPurchaseOrderItems>? StockPurchaseOrderItems { get; set; }
    }
} 