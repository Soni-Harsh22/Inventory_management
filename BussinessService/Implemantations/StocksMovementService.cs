using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Serilog;

namespace InventoryManagementSystem.BussinessService.Implemantations
{
    public class StocksMovementService : IStocksMovementService
    {
        private readonly IStocksMovementDataService _movementDataService;

        public StocksMovementService(IStocksMovementDataService movementDataService)
        {
            _movementDataService = movementDataService;
        }
        /// <summary>
        /// Handles stock movement for a product based on its status and the order status. hit every time when there is change in product table
        /// </summary>
        /// <param name="productid"></param>
        /// <param name="userId"></param>
        /// <param name="Comment"></param>
        /// <param name="quantity"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task StockMovementAsync(long productid, long userId, string Comment, int quantity, OrderStatusEnum status)
        {
            try
            {
                Log.Information("Starting stock movement for ProductId: {ProductId}, UserId: {UserId}, Status: {Status}", productid, userId, status);

                var product = await _movementDataService.GetProductByIdAsync(productid);

                if (product == null)
                {
                    Log.Warning("Product with ID {ProductId} not found", productid);
                    throw new Exception($"Product with ID {productid} not found.");
                }

                StocksMovement stockMovement = null!;

                if (product.ProductStatus == (int)StockStatusEnum.NewAdded)
                {
                    stockMovement = new StocksMovement
                    {
                        ProductId = productid,
                        MovementTypeId = (int)MovementTypeEnum.NewProductAdded,
                        UpdateBy = userId,
                        Comments = Comment,
                        MovementDate = DateTime.Now,
                        PreviousQuantity = 0,
                        QuantityChange = 0,
                        UpdateQuantity = 0
                    };
                    Log.Information("'NewProductAdded' movement for ProductId: {ProductId}", productid);
                }

                else if (product.ProductStatus == (int)StockStatusEnum.Available && OrderStatusEnum.Recived == status)
                {
                    stockMovement = new StocksMovement
                    {
                        ProductId = productid,
                        MovementTypeId = (int)MovementTypeEnum.Restock,
                        UpdateBy = userId,
                        Comments = Comment,
                        MovementDate = DateTime.Now,
                        PreviousQuantity = product.AvailableQuantity - quantity,
                        QuantityChange = quantity,
                        UpdateQuantity = product.AvailableQuantity
                    };
                    Log.Information("'Restock' movement for ProductId: {ProductId}", productid);
                }

                else if (OrderStatusEnum.Completed == status)
                {
                    stockMovement = new StocksMovement
                    {
                        ProductId = productid,
                        MovementTypeId = (int)MovementTypeEnum.SellOut,
                        UpdateBy = userId,
                        Comments = Comment,
                        MovementDate = DateTime.Now,
                        PreviousQuantity = product.AvailableQuantity + quantity,
                        QuantityChange = - quantity,
                        UpdateQuantity = product.AvailableQuantity
                    };
                    Log.Information("'SellOut' movement for ProductId: {ProductId}", productid);
                }

               else  if (OrderStatusEnum.Returned == status)
                {
                    stockMovement = new StocksMovement
                    {
                        ProductId = productid,
                        MovementTypeId = (int)MovementTypeEnum.Return,
                        UpdateBy = userId,
                        Comments = Comment,
                        MovementDate = DateTime.Now,
                        PreviousQuantity = product.AvailableQuantity - quantity,
                        QuantityChange = quantity,
                        UpdateQuantity = product.AvailableQuantity
                    };
                    Log.Information("'Return' movement for ProductId: {ProductId}", productid);
                }

                if (stockMovement != null)
                {
                    await _movementDataService.AddStockMovementAsync(stockMovement);
                    Log.Information("Stock movement successfully for ProductId: {ProductId}", productid);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while adding stock movement for ProductId: {ProductId}", productid);
                throw new Exception($"Error while adding stock movement for product ID {productid}: {ex.Message}", ex);
            }
        }
    }
}