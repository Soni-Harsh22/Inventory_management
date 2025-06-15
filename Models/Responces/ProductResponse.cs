namespace InventoryManagementSystem.Models.Responces
{
    public class ProductResponse
    {
        public long ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public string? ProductStatus { get; set; }
    }
}