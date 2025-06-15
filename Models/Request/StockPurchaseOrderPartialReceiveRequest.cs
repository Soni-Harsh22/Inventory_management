namespace InventoryManagementSystem.Models.Request
{
    public class StockPurchaseOrderPartialReceiveRequest
    {
        public long OrderId { get; set; }
        public int Status { get; set; }
        public List<long> ProductIds { get; set; }
    }
}
