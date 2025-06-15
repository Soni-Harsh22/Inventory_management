using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class StocksMovementDataService : IStocksMovementDataService
    {
        private readonly ApplicationDbContext _context;

        public StocksMovementDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddStockMovementAsync(StocksMovement movement)
        {
            _context.StocksMovement.Add(movement);
            await _context.SaveChangesAsync();
        }

        public async Task<Products?> GetProductByIdAsync(long productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
    }
}