using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.DataService.Implementations
{
    public class VendorDataService : IVendorDataService
    {
        private readonly ApplicationDbContext _context;

        public VendorDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddVendorAsync(VendorDetails vendor)
        {
            _context.VendorDetails.Add(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task<VendorDetails?> GetVendorByIdAsync(long vendorId)
        {
            return await _context.VendorDetails.FirstOrDefaultAsync(v => v.VendorId == vendorId && !v.IsDeleted);
        }

        public async Task<bool> UpdateVendorAsync(VendorDetails vendor)
        {
            _context.VendorDetails.Update(vendor);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<VendorDetails>> GetAllVendorsAsync()
        {
            return await _context.VendorDetails
                .Where(v => !v.IsDeleted)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

    }
}
