using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class StockPurchaseOrderDataService : IStockPurchaseOrderDataService
    {
        private readonly ApplicationDbContext _context;

        public StockPurchaseOrderDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddStockPurchaseOrderAsync(StockPurchaseOrders order)
        {
            _context.StockPurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<StockPurchaseOrders?> GetStockPurchaseOrderByIdAsync(long orderId)
        {
            return await _context.StockPurchaseOrders
                .Include(o => o.StockPurchaseOrderItems)
                .FirstOrDefaultAsync(o => o.StockPurchaseOrderId == orderId);
        }

        public async Task<Products?> GetProductByIdAsync(long productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task UpdateProductAsync(Products product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStockPurchaseOrderAsync(StockPurchaseOrders order)
        {
            _context.StockPurchaseOrders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<StockPurchaseOrders?> GetStockPurchaseOrderWithDetailsAsync(long orderId)
        {
            return await _context.StockPurchaseOrders
                .Include(o => o.VendorDetails)
                .Include(o => o.StockPurchaseOrderItems)!
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.StockPurchaseOrderId == orderId);
        }
        public async Task<List<StockPurchaseOrders>> GetOrdersBetweenDatesAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.StockPurchaseOrders
                .Include(o => o.VendorDetails)
                .Where(o => o.PurchaseDate >= fromDate && o.PurchaseDate <= toDate)
                .ToListAsync();
        }


    }
}