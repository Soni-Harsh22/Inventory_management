using InventoryManagementSystem.BussinessService.Implemantations;
using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route(ApiEndPoints.API_CONTROLLER)]
    public class StockPurchaseOrderController : BaseController
    {
        private readonly IStockPurchaseOrderService _orderService;

        public StockPurchaseOrderController(IStockPurchaseOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost(ApiEndPoints.PURCHASE_ORDER)]
        [Authorize(Roles = AccessPermissions.ADMIN)]
        public async Task<IActionResult> CreateStockPurchaseOrder([FromBody] StockPurchaseOrderRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var result = await _orderService.CreateStockPurchaseOrderAsync(request, userId);
            return GenerateResponse(result);
        }

        [Authorize(Roles = AccessPermissions.EMPLOYEES)]
        [HttpGet(ApiEndPoints.GET_PURCHASE_ORDER_BY_ID)]
        public async Task<IActionResult> GetStockPurchaseOrderDetails(long orderId)
        {
            var result = await _orderService.GetStockPurchaseOrderDetailsAsync(orderId);
            return GenerateResponse(result);
        }

        [Authorize(Roles = AccessPermissions.EMPLOYEES)]
        [HttpPost(ApiEndPoints.FILTER_BY_DATES)]
        public async Task<IActionResult> GetOrdersBetweenDates([FromBody] OrderFilterByDateRequest request)
        {
            var result = await _orderService.GetOrdersBetweenDatesAsync(request);
            return GenerateResponse(result);
        }

        [Authorize(Roles = AccessPermissions.EMPLOYEES)]
        [HttpPut(ApiEndPoints.UPDATE_STOCK_ORDER_STATUS)]
        public async Task<IActionResult> UpdateStockPurchaseOrderStatus([FromBody] StockPurchaseOrderPartialReceiveRequest request)
        {
            long userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var result = await _orderService.UpdateStockPurchaseOrderStatusAsync(request, userId);
            return GenerateResponse(result);
        }
    }
}