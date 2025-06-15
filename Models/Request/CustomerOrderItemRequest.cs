namespace InventoryManagementSystem.Models.Request
{
    public class CustomerOrderItemRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
