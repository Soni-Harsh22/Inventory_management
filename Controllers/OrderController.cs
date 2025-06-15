using Azure;
using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route(ApiEndPoints.API_CONTROLLER)]
    [ApiController]
    public class OrderController : BaseController
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpPost(ApiEndPoints.CUSTOMER_ORDER)]
        public async Task<IActionResult> PlaceOrder([FromBody] CustomerOrderRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var result = await _orderService.PlaceCustomerOrderAsync(request, userId);
            return GenerateResponse(result);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpPut(ApiEndPoints.RETURN_ORDER_ITEMS)]
        public async Task<IActionResult> ReturnOrderItems([FromBody] ReturnOrderItemsRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _orderService.ReturnMultipleOrderItemsAsync(request, userId);
            return GenerateResponse(response);
        }

        [HttpGet(ApiEndPoints.GET_CUSTOMER_ORDER_BY_ID)]
        [Authorize(Roles = AccessPermissions.ALL)]
        public async Task<IActionResult> GetCustomerOrderById(long orderId)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var result = await _orderService.GetCustomerOrderByIdAsync(orderId, userId);
            return GenerateResponse(result);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpPost(ApiEndPoints.GET_CUSTOMER_ORDERS_BETWEEN_DATES)]
        public async Task<IActionResult> GetCustomerOrdersBetweenDates([FromBody] CustomerOrderFilterRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var result = await _orderService.GetCustomerOrdersByDateRangeAsync(request, userId);
            return GenerateResponse(result);
        }
    }

}