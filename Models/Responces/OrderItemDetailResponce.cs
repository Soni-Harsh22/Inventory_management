namespace InventoryManagementSystem.Models.Responces
{
    public class OrderItemDetailResponce
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
