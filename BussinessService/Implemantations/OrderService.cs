using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;
using Serilog;

namespace InventoryManagementSystem.BussinessService.Implemantations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDataService _orderDataService;
        private readonly IProductDataService _productDataService;
        private readonly IStocksMovementService _stockMovement;

        public OrderService(
            IOrderDataService orderDataService,
            IProductDataService productDataService,
            IStocksMovementService stockMovement
          )
        {
            _orderDataService = orderDataService;
            _productDataService = productDataService;
            _stockMovement = stockMovement;
        }

        /// <summary>
        /// Places a customer order by validating product availability and updating stock.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> PlaceCustomerOrderAsync(CustomerOrderRequest request, long userId)
        {
            try
            {
                Log.Information("Starting order placement for userId: {UserId}", userId);

                var totalItems = request.Items.Count;
                var totalQuantity = request.Items.Sum(i => i.Quantity);

                var orderItems = new List<OrderItems>();
                decimal totalCost = 0;

                foreach (var item in request.Items)
                {
                    var product = await _productDataService.GetProductByIdAsync(item.ProductId);
                    if (product == null || product.AvailableQuantity < item.Quantity)
                    {
                        Log.Warning("Invalid or insufficient stock for product {ProductId}", item.ProductId);
                        return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, string.Format(ServiceMessageConstants.InvalidOrInsufficientStock, item.ProductId));
                    }

                    var itemTotal = product.Price * item.Quantity;
                    totalCost += itemTotal;

                    orderItems.Add(new OrderItems
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        ProductPrice = product.Price,
                        TotalPrice = itemTotal,
                        OrderItemStatus = (int)OrderStatusEnum.Completed
                    });

                    product.AvailableQuantity -= item.Quantity;

                    if (product.AvailableQuantity == 0)
                    {
                        product.ProductStatus = (int)StockStatusEnum.OutOfStock;
                        Log.Information("Product {ProductId} is now out of stock", item.ProductId);
                    }
                    else if (product.AvailableQuantity < product.MinimumRequiredQuantity)
                    {
                        product.ProductStatus = (int)StockStatusEnum.LowStock;
                        Log.Information("Product {ProductId} is now low on stock", item.ProductId);
                    }

                    await _productDataService.UpdateProductAsync(product);
                    await _stockMovement.StockMovementAsync(item.ProductId, userId, null!, item.Quantity, OrderStatusEnum.Completed);
                }

                var order = new Orders
                {
                    UserId = userId,
                    NumberOfItems = totalItems,
                    TotalQuantity = totalQuantity,
                    TotalCost = totalCost,
                    OrderStatus = (int)OrderStatusEnum.Completed,
                    PaymentType = (int)PaymentTypeEnum.Postpaid,
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = (int)PaymentStatusEnum.Paid,
                    PaymentDate = DateTime.Now,
                    OrderItems = orderItems,
                    UpdatedBy = (int)Role.System,
                };

                await _orderDataService.CreateOrderAsync(order);
                Log.Information("Order successfully placed for userId: {UserId}", userId);

                return ApiResponse<string>.CreateResponse(ResponseCode.Created, ServiceMessageConstants.OrderPlacedSuccessfully);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error placing order for userId: {UserId}", userId);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, ServiceMessageConstants.ErrorPlacingOrder);
            }
        }
        /// <summary>
        /// Processes the return of multiple order items by updating their status and adjusting stock levels.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> ReturnMultipleOrderItemsAsync(ReturnOrderItemsRequest request, long updatedBy)
        {
            try
            {
                Log.Information("Processing return for OrderId {OrderId}", request.OrderId);

                var orderItems = await _orderDataService.GetOrderItemsAsync(request.OrderId, request.ProductIds);
                var orderItemsProducts = await _orderDataService.GetOrderItemsByIdAsync(request.OrderId);

                if (orderItems == null || !orderItems.Any())
                {
                    Log.Warning("No matching items found for return in OrderId {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, ServiceMessageConstants.NoMatchingOrderItemsFound);
                }

                foreach (var item in orderItems)
                {
                    if (item.OrderItemStatus == (int)OrderStatusEnum.Returned)
                    {
                        Log.Warning($"Item {item.ProductId} in OrderId {request.OrderId} has already been returned");
                        var message = string.Format(ServiceMessageConstants.OrderALreadyReturn, item.ProductId, request.OrderId);
                        return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, message);
                    }
                }

                var updatedItems = new List<OrderItems>();
                foreach (var item in orderItems)
                {
                    item.OrderItemStatus = (int)OrderStatusEnum.Returned;
                    item.OrderDate = DateTime.Now;
                    updatedItems.Add(item);

                    var product = await _orderDataService.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.AvailableQuantity += item.Quantity;

                        var isUpdate = await _productDataService.UpdateProductAsync(product);
                        if (isUpdate)
                        {
                            await _stockMovement.StockMovementAsync(item.ProductId, updatedBy, null!, item.Quantity, OrderStatusEnum.Returned);
                        }
                    }
                }

                var order = await _orderDataService.GetOrderByIdAsync(request.OrderId);
                if (order != null)
                {
                    order.UpdatedBy = (int)updatedBy;
                    order.UpdatedDate = DateTime.Now;
                    if(orderItemsProducts.Count > orderItems.Count)
                    {
                        order.OrderStatus = (int)OrderStatusEnum.ParcialReturn;
                    }
                    else
                    {
                        order.OrderStatus = (int)OrderStatusEnum.Returned;
                    }
                }

                var result = await _orderDataService.UpdateOrderItemsAndProductsAsync(updatedItems, order!);
                if (result)
                {
                    Log.Information("Return processed successfully for OrderId {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.Ok, ServiceMessageConstants.ReturnSuccess);
                }
                else {
                    Log.Error("Failed to return items for OrderId {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, ServiceMessageConstants.ReturnFailed);
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error processing return for OrderId {OrderId}", request.OrderId);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, ServiceMessageConstants.ErrorProcessingReturn);
            }
        }
        /// <summary>
        /// Fetches order details for a specific order by its ID and user ID.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<CustomerOrderDetailsResponse>> GetCustomerOrderByIdAsync(long orderId, long userId)
        {
            try
            {
                Log.Information("Fetching order details for OrderId {OrderId}, UserId {UserId}", orderId, userId);

                var order = await _orderDataService.GetOrderDetailsByIdAsync(orderId, userId);
                if (order == null)
                {
                    Log.Warning("Order not found for OrderId {OrderId}", orderId);
                    return ApiResponse<CustomerOrderDetailsResponse>.CreateResponse(ResponseCode.NotFound, ServiceMessageConstants.OrderNotFound, null);
                }

                var response = new CustomerOrderDetailsResponse
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    NumberOfItems = order.NumberOfItems,
                    TotalQuantity = order.TotalQuantity,
                    TotalCost = order.TotalCost,
                    OrderStatus = order.OrderStatusNavigation.Name,
                    PaymentType = order.PaymentTypeNavigation.Name,
                    PaymentMethod = order.PaymentMethodNavigation.Name,
                    PaymentStatus = order.PaymentStatusNavigation.Name,

                    Items = order.OrderItems.Select(i => new OrderItemDetailResponce
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name!,
                        Quantity = i.Quantity,
                        ProductPrice = i.ProductPrice,
                        TotalPrice = i.TotalPrice,
                        Status = Enum.GetName(typeof(OrderStatusEnum), i.OrderItemStatus)!
                    }).ToList()
                };

                return ApiResponse<CustomerOrderDetailsResponse>.CreateResponse(ResponseCode.Ok, "", response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching order details for OrderId {OrderId}, UserId {UserId}", orderId, userId);
                return ApiResponse<CustomerOrderDetailsResponse>.CreateResponse(ResponseCode.InternalServerError, ServiceMessageConstants.ErrorFetchingOrderDetails);
            }
        }

        /// <summary>
        /// Fetches a list of customer orders within a specified date range.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<CustomerOrderSummaryResponse>>> GetCustomerOrdersByDateRangeAsync(CustomerOrderFilterRequest request, long userId)
        {
            try
            {
                Log.Information("Fetching orders for UserId {UserId} between {StartMonth}/{StartYear} and {EndMonth}/{EndYear}",
                    userId, request.StartMonth, request.StartYear, request.EndMonth, request.EndYear);

                if (request.StartMonth < 1 || request.StartMonth > 12 || request.EndMonth < 1 || request.EndMonth > 12)
                    return ApiResponse<List<CustomerOrderSummaryResponse>>.CreateResponse(ResponseCode.BadRequest, ServiceMessageConstants.InvalidMonth);

                if (request.StartYear > DateTime.Now.Year || request.EndYear > DateTime.Now.Year)
                    return ApiResponse<List<CustomerOrderSummaryResponse>>.CreateResponse(ResponseCode.BadRequest, ServiceMessageConstants.FutureYear);

                var startDate = new DateTime(request.StartYear, request.StartMonth, 1);
                var endDate = new DateTime(request.EndYear, request.EndMonth, DateTime.DaysInMonth(request.EndYear, request.EndMonth));

                if (startDate > endDate)
                    return ApiResponse<List<CustomerOrderSummaryResponse>>.CreateResponse(ResponseCode.BadRequest, ServiceMessageConstants.INVALID_DATE_RANGE);

                var orders = await _orderDataService.GetCustomerOrdersBetweenDatesAsync(userId, startDate, endDate);
                if (orders.Count == 0)
                {
                    Log.Information("No orders found between the specified dates");
                    return ApiResponse<List<CustomerOrderSummaryResponse>>.CreateResponse(ResponseCode.NotFound, ServiceMessageConstants.ORDER_NOT_FOUND_FOR_DATES);
                }

                var response = orders.Select(o => new CustomerOrderSummaryResponse
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalItems = o.NumberOfItems,
                    TotalCost = o.TotalCost,
                    OrderStatus = o.OrderStatusNavigation.Name
                }).ToList();

                Log.Information("Retrieved {Count} orders for user {UserId}", response.Count, userId);
                return ApiResponse<List<CustomerOrderSummaryResponse>>.CreateResponse(ResponseCode.Ok, "", response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching orders for UserId {UserId}", userId);
                return ApiResponse<List<CustomerOrderSummaryResponse>>.CreateResponse(ResponseCode.InternalServerError, ServiceMessageConstants.ERRORFETCHINGORDERS);
            }
        }
    }
}