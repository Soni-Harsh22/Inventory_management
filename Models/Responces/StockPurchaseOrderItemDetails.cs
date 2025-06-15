namespace InventoryManagementSystem.Models.Responces
{
    public class StockPurchaseOrderItemDetails
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderItemStatus { get; set; }
        public DateTime? OrderReceivedDate { get; set; }
    }
}
