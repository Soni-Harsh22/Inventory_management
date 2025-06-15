using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class UserDataService : IUserDataService
    {
        private readonly ApplicationDbContext _context;

        public UserDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task AddUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsValidRoleIdAsync(int roleId)
        {
            return await _context.Roles.AnyAsync(r => r.RoleId == roleId);
        }
        public async Task<Users?> GetUserByIdAsync(long userId)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId && u.IsDeleted == false);
        }
    }
}