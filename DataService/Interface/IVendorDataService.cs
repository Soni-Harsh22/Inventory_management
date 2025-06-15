using InventoryManagementSystem.Models.Entity;

namespace InventoryManagementSystem.DataService.Interface
{
    public interface IVendorDataService
    {
        Task AddVendorAsync(VendorDetails vendor);
        Task<VendorDetails?> GetVendorByIdAsync(long vendorId);
        Task<bool> UpdateVendorAsync(VendorDetails vendor);
        Task<List<VendorDetails>> GetAllVendorsAsync();
    }
}