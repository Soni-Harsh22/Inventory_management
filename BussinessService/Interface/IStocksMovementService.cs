using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface IStocksMovementService
    {
        Task StockMovementAsync(long ProductId, long userId, string Comment, int quantity,OrderStatusEnum status);
    }
}