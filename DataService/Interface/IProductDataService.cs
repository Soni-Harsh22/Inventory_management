using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface IProductDataService
    {
        Task AddProductAsync(Products product);
        Task<Products?> GetProductByIdAsync(long productId);
        Task<bool> UpdateProductAsync(Products product);
        Task<List<Products>> GetAllProductsAsync();
        Task<List<Products>> GetLowStockProductsAsync();
        Task<List<Products>> SearchProductsByNameAsync(string keyword);
        Task<List<Products>> GetProductByCategoryAsync(int categoryId);

    }
}
