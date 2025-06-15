using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface IProductService
    {
        Task<ApiResponse<string>> AddProductAsync(AddProductRequest request, long userId);
        Task<ApiResponse<string>> SoftDeleteProductAsync(DeleteProductRequest request, long userId);
        Task<ApiResponse<string>> UpdateProductAsync(UpdateProductRequest request, long userId);
        Task<ApiResponse<List<ProductResponse>>> GetAllProductsAsync();
        Task<ApiResponse<List<ProductResponse>>> GetLowStockProductsAsync();
        Task<ApiResponse<List<ProductResponse>>> SearchProductsAsync(string keyword);
        Task<ApiResponse<List<ProductResponse>>> GetProductByCategoryAsync(int categoryId);
        Task<ApiResponse<ProductResponse>> GetProductByIdAsync(long productId);
    }
}