using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> PlaceCustomerOrderAsync(CustomerOrderRequest request, long userId);
        Task<ApiResponse<string>> ReturnMultipleOrderItemsAsync(ReturnOrderItemsRequest request, long updatedBy);
        Task<ApiResponse<CustomerOrderDetailsResponse>> GetCustomerOrderByIdAsync(long orderId, long userId);
        Task<ApiResponse<List<CustomerOrderSummaryResponse>>> GetCustomerOrdersByDateRangeAsync(CustomerOrderFilterRequest request, long userId);


    }
}
