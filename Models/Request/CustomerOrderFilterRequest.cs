namespace InventoryManagementSystem.Models.Request
{
    public class CustomerOrderFilterRequest
    {
        public int StartMonth { get; set; }    
        public int StartYear { get; set; }

        public int EndMonth { get; set; }      
        public int EndYear { get; set; }
    }

}
