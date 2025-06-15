using System.Text.Json.Serialization;

namespace InventoryManagementSystem.Models.Request
{
    public class StockPurchaseOrderItemRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        [JsonIgnore]
        public decimal TotalPrice { get; set; }
    }
}