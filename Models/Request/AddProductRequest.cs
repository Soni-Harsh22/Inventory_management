using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models.Request
{
    public class AddProductRequest
    {
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int MinimumRequiredQuantity { get; set; }

        public string? Comment { get; set; }

    }
}
