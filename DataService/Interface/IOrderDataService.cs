using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface IOrderDataService
    {
        Task CreateOrderAsync(Orders order);
        Task<List<OrderItems>> GetOrderItemsAsync(long orderId, List<long> productIds);
        Task<List<OrderItems>> GetOrderItemsByIdAsync(long orderId);
        Task<Orders?> GetOrderByIdAsync(long orderId);
        Task<Products?> GetProductByIdAsync(long productId);
        Task<bool> UpdateOrderItemsAndProductsAsync(List<OrderItems> itemsToUpdate, Orders order);
        Task<Orders?> GetOrderDetailsByIdAsync(long orderId, long userId);
        Task<List<Orders>> GetCustomerOrdersBetweenDatesAsync(long userId, DateTime startDate, DateTime endDate);
    }
}