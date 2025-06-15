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
    public class StockPurchaseOrderService : IStockPurchaseOrderService
    {
        private readonly IStockPurchaseOrderDataService _dataService;
        private readonly IStocksMovementService _stockMovement;
        private readonly IVendorDataService _vendorDataService;
        private readonly IProductDataService _productDataService;



        public StockPurchaseOrderService(IStockPurchaseOrderDataService dataService, IStocksMovementService stockMovement, IVendorDataService vendorDataService, IProductDataService productDataService)
        {
            _dataService = dataService;
            _stockMovement = stockMovement;
            _vendorDataService = vendorDataService;
            _productDataService = productDataService;
        }
        /// <summary>
        /// Creates a new stock purchase order.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> CreateStockPurchaseOrderAsync(StockPurchaseOrderRequest request, long userId)
        {
            try
            {
                Log.Information("Creating stock purchase order for VendorId: {VendorId} by UserId: {UserId}", request.VendorId, userId);

                int numberOfItems = request.Items!.Count;
                int totalQuantity = request.Items.Sum(i => i.Quantity);
                decimal totalPrice = request.Items.Sum(i => i.ProductPrice * i.Quantity);

                var vendorExist = await _vendorDataService.GetVendorByIdAsync(request.VendorId);
                if (vendorExist == null)
                {
                    Log.Warning("Vendor not found for VendorId: {VendorId}", request.VendorId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, StockPurchaseOrderConstants.VENDOR_NOT_FOUND);
                }

                foreach (var item in request.Items)
                {
                    var productExist = await _productDataService.GetProductByIdAsync(item.ProductId);
                    if (productExist == null)
                    {
                        Log.Warning("Product not found for ProductId: {ProductId}", item.ProductId);
                        return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, StockPurchaseOrderConstants.PRODUCT_NOT_FOUND);
                    }
                }

                var order = new StockPurchaseOrders
                {
                    NumberOfItem = numberOfItems,
                    TotalNumberOfQuantity = totalQuantity,
                    TotalPrice = totalPrice,
                    PurchaseBy = userId,
                    VendorId = request.VendorId,
                    OrderStatusId = (int)OrderStatusEnum.Pending,
                    PaymentTypeId = request.PaymentTypeId,
                    PaymentMethodId = request.PaymentMethodId,
                    UpdatedBy = userId,
                    StockPurchaseOrderItems = request.Items.Select(i => new StockPurchaseOrderItems
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        ProductPrice = i.ProductPrice,
                        TotalPrice = i.ProductPrice * i.Quantity,
                        OrderItemStatus = (int)OrderStatusEnum.Pending,
                    }).ToList()
                };

                order.PaymentStatusId = order.PaymentTypeId == (int)PaymentTypeEnum.Prepaid
                    ? (int)PaymentStatusEnum.Paid
                    : (int)PaymentStatusEnum.Pending;

                await _dataService.AddStockPurchaseOrderAsync(order);
               
                Log.Information("Stock purchase order created successfully for VendorId: {VendorId}", request.VendorId);
                foreach (var item in request.Items)
                {
                    var productExist = await _productDataService.GetProductByIdAsync(item.ProductId);
                    if (productExist == null)
                    {
                        Log.Warning("Product not found for ProductId: {ProductId}", item.ProductId);
                        return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, StockPurchaseOrderConstants.PRODUCT_NOT_FOUND);
                    }

                    if (productExist.Price < item.ProductPrice)
                    {
                        Log.Warning("Product price mismatch for ProductId: {ProductId}", item.ProductId);
                        return ApiResponse<string>.CreateResponse(ResponseCode.Created, StockPurchaseOrderConstants.PURCHASE_PRICE_GREATER_PRODUCTPRICE);
                    }
                }

                return ApiResponse<string>.CreateResponse(ResponseCode.Created, StockPurchaseOrderConstants.STOCK_PURCHASE_ORDER_CREATED);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating stock purchase order for VendorId: {VendorId}", request.VendorId);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"{StockPurchaseOrderConstants.ERROR_CREATING_STOCK_PURCHASE_ORDER}: {ex.Message}");
            }
        }
        /// <summary>
        /// Fetches stock purchase order details by order ID.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<StockPurchaseOrderDetailsResponse>> GetStockPurchaseOrderDetailsAsync(long orderId)
        {
            try
            {
                Log.Information("Fetching stock purchase order details for OrderId: {OrderId}", orderId);

                var order = await _dataService.GetStockPurchaseOrderWithDetailsAsync(orderId);
                if (order == null)
                {
                    Log.Warning("Order not found with OrderId: {OrderId}", orderId);
                    return ApiResponse<StockPurchaseOrderDetailsResponse>.CreateResponse(ResponseCode.NotFound, StockPurchaseOrderConstants.ORDER_NOT_FOUND);
                }

                var response = new StockPurchaseOrderDetailsResponse
                {
                    OrderId = order.StockPurchaseOrderId,
                    VendorName = order.VendorDetails?.StoreName!,
                    NumberOfItem = order.NumberOfItem,
                    TotalQuantity = order.TotalNumberOfQuantity,
                    TotalPrice = order.TotalPrice,
                    OrderStatus = Enum.GetName(typeof(OrderStatusEnum), order.OrderStatusId)!,
                    CreatedAt = order.PurchaseDate,
                    Items = order.StockPurchaseOrderItems!.Select(item => new StockPurchaseOrderItemDetails
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product?.Name!,
                        Quantity = item.Quantity,
                        ProductPrice = item.ProductPrice,
                        TotalPrice = item.TotalPrice,
                        OrderItemStatus = Enum.GetName(typeof(OrderStatusEnum), item.OrderItemStatus)!,
                        OrderReceivedDate = item.OrderRecivedDate
                    }).ToList()
                };

                Log.Information("Successfully fetched stock purchase order details for OrderId: {OrderId}", orderId);
                return ApiResponse<StockPurchaseOrderDetailsResponse>.CreateResponse(ResponseCode.Ok, StockPurchaseOrderConstants.FETCH_PURCHASE_ORDER_DETAIL, response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching stock purchase order details for OrderId: {OrderId}", orderId);
                return ApiResponse<StockPurchaseOrderDetailsResponse>.CreateResponse(ResponseCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Fetches stock purchase orders between specified dates.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<StockPurchaseOrderSummaryResponse>>> GetOrdersBetweenDatesAsync(OrderFilterByDateRequest request)
        {
            try
            {
                Log.Information("Fetching orders between dates: {FromMonth}/{FromYear} - {ToMonth}/{ToYear}", request.FromMonth, request.FromYear, request.ToMonth, request.ToYear);

                if (request.FromMonth < 1 || request.FromMonth > 12 || request.ToMonth < 1 || request.ToMonth > 12)
                {
                    Log.Warning("Invalid month input");
                    return ApiResponse<List<StockPurchaseOrderSummaryResponse>>.CreateResponse(ResponseCode.BadRequest, StockPurchaseOrderConstants.INVALID_MONTH);
                }

                int currentYear = DateTime.Now.Year;
                if (request.FromYear > currentYear || request.ToYear > currentYear)
                {
                    Log.Warning("Invalid year input");
                    return ApiResponse<List<StockPurchaseOrderSummaryResponse>>.CreateResponse(ResponseCode.BadRequest, StockPurchaseOrderConstants.FUTURE_YEAR);
                }

                var fromDate = new DateTime(request.FromYear, request.FromMonth, 1);
                var toDate = new DateTime(request.ToYear, request.ToMonth, DateTime.DaysInMonth(request.ToYear, request.ToMonth));

                if (fromDate > toDate)
                {
                    Log.Warning("From date is after To date");
                    return ApiResponse<List<StockPurchaseOrderSummaryResponse>>.CreateResponse(ResponseCode.BadRequest, StockPurchaseOrderConstants.INVALID_DATE_RANGE);
                }

                var orders = await _dataService.GetOrdersBetweenDatesAsync(fromDate, toDate);
                if(orders.Count==0)
                {
                    Log.Information("No orders found between the specified dates");
                    return ApiResponse<List<StockPurchaseOrderSummaryResponse>>.CreateResponse(ResponseCode.NotFound, StockPurchaseOrderConstants.ORDER_NOT_FOUND_FOR_DATES);
                }
                var response = orders.Select(order => new StockPurchaseOrderSummaryResponse
                {
                    OrderId = order.StockPurchaseOrderId,
                    VendorName = order.VendorDetails?.StoreName ?? "Unknown Vendor",
                    TotalItems = order.NumberOfItem,
                    TotalQuantity = order.TotalNumberOfQuantity,
                    TotalPrice = order.TotalPrice,
                    OrderStatus = Enum.GetName(typeof(OrderStatusEnum), order.OrderStatusId)!,
                    CreatedAt = order.PurchaseDate
                }).ToList();

                Log.Information("Fetched {Count} orders", response.Count);
                return ApiResponse<List<StockPurchaseOrderSummaryResponse>>.CreateResponse(ResponseCode.Ok, "", response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error fetching orders between dates");
                return ApiResponse<List<StockPurchaseOrderSummaryResponse>>.CreateResponse(ResponseCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Updates the status of a stock purchase order and alos add the quantity in the productes table.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> UpdateStockPurchaseOrderStatusAsync(StockPurchaseOrderPartialReceiveRequest request, long userId)
        {
            try
            {
                Log.Information("Partial update of order status for OrderId: {OrderId} by UserId: {UserId}", request.OrderId, userId);

                var order = await _dataService.GetStockPurchaseOrderByIdAsync(request.OrderId);
                if (order == null)
                {
                    Log.Warning("Order not found with OrderId: {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, StockPurchaseOrderConstants.STOCK_PURCHASE_ORDER_NOT_FOUND);
                }

                if (order.OrderStatusId == (int)OrderStatusEnum.Recived || order.OrderStatusId == (int)OrderStatusEnum.Cancelled)
                {
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, StockPurchaseOrderConstants.ALREADY_RETURN_CANCLE);
                }

                order.UpdatedBy = userId;
                order.UpdatedAt = DateTime.Now;

                if (request.Status == (int)OrderStatusEnum.Recived)
                {
                    order.OrderStatusId = request.Status;
                    order.PaymentStatusId = (int)PaymentStatusEnum.Paid;
                    order.PaymentDate = DateTime.Now;
                    order.OrderRecivedDate = DateTime.Now;

                    foreach (var item in order.StockPurchaseOrderItems!)
                    {
                        item.OrderItemStatus = (int)OrderStatusEnum.Recived;
                        item.OrderRecivedDate = DateTime.Now;

                        var product = await _dataService.GetProductByIdAsync(item.ProductId);
                        if (product != null)
                        {
                            product.AvailableQuantity += item.Quantity;
                            product.UpdatedBy = userId;
                            product.UpdatedAt = DateTime.Now;
                            product.LastRestockAt = DateTime.Now;
                            product.ProductStatus = (int)StockStatusEnum.Available;

                            await _dataService.UpdateProductAsync(product);
                            await _stockMovement.StockMovementAsync(item.ProductId, userId, null!, item.Quantity, OrderStatusEnum.Recived);
                        }
                    }

                    await _dataService.UpdateStockPurchaseOrderAsync(order);
                    Log.Information("Stock purchase order fully received for OrderId: {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.Ok, StockPurchaseOrderConstants.STOCK_PURCHASE_ORDER_FULLY_RECEIVED);
                }
                else if (request.Status == (int)OrderStatusEnum.Cancelled)
                {
                    order.OrderStatusId = request.Status;

                    foreach (var item in order.StockPurchaseOrderItems!)
                    {
                        item.OrderItemStatus = (int)OrderStatusEnum.Cancelled;
                        item.OrderRecivedDate = DateTime.Now;
                    }

                    await _dataService.UpdateStockPurchaseOrderAsync(order);
                    Log.Information("Order cancelled for OrderId: {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.Ok, StockPurchaseOrderConstants.STOCK_PURCHASE_ORDER_CANCELLED);
                }
                else if (request.Status == (int)OrderStatusEnum.Incomplete)
                {
                    if (request.ProductIds == null || !request.ProductIds.Any())
                    {
                        Log.Warning("No product IDs provided for incomplete status for OrderId: {OrderId}", request.OrderId);
                        return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, StockPurchaseOrderConstants.NO_PRODUCT_IDS_PROVIDED);
                    }

                    order.OrderStatusId = request.Status;
                    order.PaymentStatusId = (int)PaymentStatusEnum.Pending;
                    order.PaymentDate = DateTime.Now;
                    order.OrderRecivedDate = DateTime.Now;

                    foreach (var item in order.StockPurchaseOrderItems!)
                    {
                        if (request.ProductIds.Contains(item.ProductId))
                        {
                            item.OrderItemStatus = (int)OrderStatusEnum.Recived;
                            item.OrderRecivedDate = DateTime.Now;

                            var product = await _dataService.GetProductByIdAsync(item.ProductId);
                            if (product != null)
                            {
                                product.AvailableQuantity += item.Quantity;
                                product.UpdatedBy = userId;
                                product.UpdatedAt = DateTime.Now;
                                product.LastRestockAt = DateTime.Now;
                                product.ProductStatus = (int)StockStatusEnum.Available;

                                await _dataService.UpdateProductAsync(product);
                                await _stockMovement.StockMovementAsync(item.ProductId, userId, null!, item.Quantity, OrderStatusEnum.Recived);
                            }
                        }
                    }

                    await _dataService.UpdateStockPurchaseOrderAsync(order);
                    Log.Information("Stock purchase order marked as Incomplete with partial receipt for OrderId: {OrderId}", request.OrderId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.Ok, StockPurchaseOrderConstants.STOCK_PURCHASE_ORDER_INCOMPLETE);
                }

                Log.Warning("Invalid status value provided: {Status} for OrderId: {OrderId}", request.Status, request.OrderId);
                return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, StockPurchaseOrderConstants.INVALID_STATUS_VALUE);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating stock purchase order for OrderId: {OrderId}", request.OrderId);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"{StockPurchaseOrderConstants.ERROR_UPDATING_STOCK_PURCHASE_ORDER}: {ex.Message}");
            }
        }    
    }
}