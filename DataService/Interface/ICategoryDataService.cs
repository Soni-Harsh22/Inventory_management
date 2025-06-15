using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface ICategoryDataService
    {
        Task AddCategoryAsync(Category category);
        Task<Category?> GetCategoryByNameAsync(string name);

        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int categoryId);
        Task UpdateCategoryAsync(Category category);


    }
}
