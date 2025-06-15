namespace InventoryManagementSystem.Models.Request
{
    public class UpdateProductRequest
    {
        public long ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Price { get; set; }
        public int? MinimumRequiredQuantity { get; set; }
    }
}
