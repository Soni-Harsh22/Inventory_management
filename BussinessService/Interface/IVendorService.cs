using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;

namespace InventoryManagementSystem.BussinessService.Interface
{
    public interface IVendorService
    {
        Task<ApiResponse<string>> AddVendorAsync(VendorRequest request, long userId);
        Task<ApiResponse<string>> DeleteVendorAsync(DeleteVendorRequest request, long userId);
        Task<ApiResponse<List<VendorDetails>>> GetAllVendorsAsync();
        Task<ApiResponse<string>> UpdateVendorAsync(UpdateVendorRequest request, long userId);


    }
}
