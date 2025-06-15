namespace InventoryManagementSystem.Models.Request
{
    public class CustomerOrderRequest
    {
        public List<CustomerOrderItemRequest> Items { get; set; }
        public int PaymentMethod { get; set; }
    }
}
