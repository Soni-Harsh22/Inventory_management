namespace InventoryManagementSystem.Models.Responces
{
    public class OrderFilterByDateRequest
    {
        public int FromMonth { get; set; }
        public int FromYear { get; set; }
        public int ToMonth { get; set; }
        public int ToYear { get; set; }
    }
}
