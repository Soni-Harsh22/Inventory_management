using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface IUserDataService
    {
        Task<Users?> GetUserByEmailAsync(string email);
        Task AddUserAsync(Users user);
        Task<bool> IsValidRoleIdAsync(int roleId);
        Task<Users?> GetUserByIdAsync(long userId);


    }
}
