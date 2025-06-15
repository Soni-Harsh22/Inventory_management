using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class CategoryDataService : ICategoryDataService
    {
        private readonly ApplicationDbContext _context;

        public CategoryDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _context.Category
                .FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Category
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Category
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId && !c.IsDeleted);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
