namespace InventoryManagementSystem.Models.Responces
{
    public class CustomerOrderSummaryResponse
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalCost { get; set; }
        public string OrderStatus { get; set; }
    }
}
