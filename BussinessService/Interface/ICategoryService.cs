using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface ICategoryService
    {
        Task<ApiResponse<string>> AddCategoryAsync(AddCategoryRequest request, long userId);
        Task<ApiResponse<List<Category>>> GetAllCategoriesAsync();
        Task<ApiResponse<string>> DeleteCategoryAsync(int categoryId, long userId);
    }
}
