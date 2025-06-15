namespace InventoryManagementSystem.Models.Request
{
    public class ReturnOrderItemsRequest
    {
        public long OrderId { get; set; }
        public List<long> ProductIds { get; set; }
    }
}
