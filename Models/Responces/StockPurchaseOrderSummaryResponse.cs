namespace InventoryManagementSystem.Models.Responces
{
    public class StockPurchaseOrderSummaryResponse
    {
        public long OrderId { get; set; }
        public string VendorName { get; set; }
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
