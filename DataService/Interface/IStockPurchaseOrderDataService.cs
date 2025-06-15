using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface IStockPurchaseOrderDataService
    {
        Task AddStockPurchaseOrderAsync(StockPurchaseOrders order);
        Task<StockPurchaseOrders?> GetStockPurchaseOrderByIdAsync(long orderId);
        Task<Products?> GetProductByIdAsync(long productId);
        Task UpdateProductAsync(Products product);
        Task UpdateStockPurchaseOrderAsync(StockPurchaseOrders order);
        Task<StockPurchaseOrders?> GetStockPurchaseOrderWithDetailsAsync(long orderId);
        Task<List<StockPurchaseOrders>> GetOrdersBetweenDatesAsync(DateTime fromDate, DateTime toDate);

    }
}