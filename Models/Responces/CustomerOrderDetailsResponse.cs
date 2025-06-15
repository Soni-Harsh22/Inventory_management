namespace InventoryManagementSystem.Models.Responces
{
    public class CustomerOrderDetailsResponse
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int NumberOfItems { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public List<OrderItemDetailResponce> Items { get; set; }
    }
}
