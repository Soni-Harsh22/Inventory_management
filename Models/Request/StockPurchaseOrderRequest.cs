namespace InventoryManagementSystem.Models.Request
{
    public class StockPurchaseOrderRequest
    {
        public long VendorId { get; set; }
        public int PaymentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public List<StockPurchaseOrderItemRequest>? Items { get; set; }
    }
}