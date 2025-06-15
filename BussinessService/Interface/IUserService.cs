using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface IUserService
    {
        Task<ApiResponse<string>> RegisterAsync(UserRegisterRequest request);
        Task<ApiResponse<UserLoginResponce>> LoginAsync(UserLoginRequest request);
        Task<ApiResponse<UserLoginResponce>> RefreshTokenAsync(string refreshToken);
    }
}
