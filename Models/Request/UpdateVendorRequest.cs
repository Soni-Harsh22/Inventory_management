namespace InventoryManagementSystem.Models.Request
{
    public class UpdateVendorRequest
    {
        public long VendorId { get; set; } 
        public string? Name { get; set; }
        public string? StoreName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Pincode { get; set; }
    }
}