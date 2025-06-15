using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface IStockPurchaseOrderService
    {
        Task<ApiResponse<string>> CreateStockPurchaseOrderAsync(StockPurchaseOrderRequest request, long userId);
        Task<ApiResponse<StockPurchaseOrderDetailsResponse>> GetStockPurchaseOrderDetailsAsync(long orderId);
        Task<ApiResponse<List<StockPurchaseOrderSummaryResponse>>> GetOrdersBetweenDatesAsync(OrderFilterByDateRequest request);
        Task<ApiResponse<string>> UpdateStockPurchaseOrderStatusAsync(StockPurchaseOrderPartialReceiveRequest request, long userId);
    }
}