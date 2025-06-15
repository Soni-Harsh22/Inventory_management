using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class OrderDataService : IOrderDataService
    {
        private readonly ApplicationDbContext _context;

        public OrderDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Orders order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderItems>> GetOrderItemsAsync(long orderId, List<long> productIds)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == orderId && productIds.Contains(x.ProductId))
                .ToListAsync();
        }

        public async Task<List<OrderItems>> GetOrderItemsByIdAsync(long orderId)
        {
            return await _context.OrderItems
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<Orders?> GetOrderByIdAsync(long orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<Products?> GetProductByIdAsync(long productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<bool> UpdateOrderItemsAndProductsAsync(List<OrderItems> itemsToUpdate, Orders order)
        {
            _context.OrderItems.UpdateRange(itemsToUpdate);
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Orders?> GetOrderDetailsByIdAsync(long orderId, long userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderStatusNavigation)
                .Include(o => o.PaymentTypeNavigation)
                .Include(o => o.PaymentMethodNavigation)
                .Include(o => o.PaymentStatusNavigation)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);
        }

        public async Task<List<Orders>> GetCustomerOrdersBetweenDatesAsync(long userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Include(o => o.OrderStatusNavigation)
                .Where(o => o.UserId == userId && o.OrderDate >= startDate && o.OrderDate <= endDate)
                .ToListAsync();
        }

    }
}