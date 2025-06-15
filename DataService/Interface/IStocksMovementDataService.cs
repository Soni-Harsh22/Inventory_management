using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface IStocksMovementDataService
    {
        Task AddStockMovementAsync(StocksMovement movement);
        Task<Products?> GetProductByIdAsync(long productId);
    }
}