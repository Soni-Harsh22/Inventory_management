namespace InventoryManagementSystem.Models.Responces
{
    public class StockPurchaseOrderDetailsResponse
    {
        public long OrderId { get; set; }
        public string VendorName { get; set; }
        public int NumberOfItem { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<StockPurchaseOrderItemDetails> Items { get; set; }
    }
}
