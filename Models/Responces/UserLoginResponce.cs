namespace InventoryManagementSystem.Models.Responces
{
    public class UserLoginResponce
    {
        public long UserId { get; set; }
        public string? Name { get; set; }
        public int RoleId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
