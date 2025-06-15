using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class ProductDataService : IProductDataService
    {
        private readonly ApplicationDbContext _context;

        public ProductDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Products?> GetProductByIdAsync(long productId)
        {

            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId && !p.IsDeleted);
        }
        public async Task<bool> UpdateProductAsync(Products product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Products>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted &&
                       (p.AvailableQuantity <= p.MinimumRequiredQuantity))
                        .ToListAsync();

        }

        public async Task<List<Products>> SearchProductsByNameAsync(string keyword)
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && p.Name !.ToLower().Contains(keyword.ToLower()))
                .ToListAsync();
        }
        public async Task<List<Products>> GetProductByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => !p.IsDeleted && p.CategoryId == categoryId)
                .ToListAsync();
        }


    }
}