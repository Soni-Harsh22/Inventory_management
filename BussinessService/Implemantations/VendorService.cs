using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;
using Serilog;

namespace InventoryManagementSystem.BussinessService.Implemantations
{
    public class VendorService : IVendorService
    {
        private readonly IVendorDataService _vendorDataService;
        public VendorService(IVendorDataService vendorDataService)
        {
            _vendorDataService = vendorDataService;
        }
        /// <summary>
        /// Adds a new vendor to the system.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> AddVendorAsync(VendorRequest request, long userId)
        {
            try
            {
                Log.Information("Adding new vendor: {VendorName}", request.Name);

                var vendor = new VendorDetails
                {
                    Name = request.Name,
                    StoreName = request.StoreName,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address = request.Address,
                    City = request.City,
                    State = request.State,
                    Country = request.Country,
                    Pincode = request.Pincode,
                    CreatedBy = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedBy = (int)Role.System,
                    IsDeleted = false
                };

                await _vendorDataService.AddVendorAsync(vendor);
                Log.Information("Vendor added successfully: {VendorName}", vendor.Name);
                return ApiResponse<string>.CreateResponse(ResponseCode.Created, VendorConstants.VENDOR_ADDED_SUCCESSFULLY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding vendor: {VendorName}", request.Name);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"{VendorConstants.ERROR_ADDING_VENDOR}: {ex.Message}");
            }
        }
        /// <summary>
        /// Deletes a vendor from the system.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> DeleteVendorAsync(DeleteVendorRequest request, long userId)
        {
            try
            {
                Log.Information("Deleting vendor with ID: {VendorId}", request.VendorId);

                if (request.VendorId <= 0)
                {
                    Log.Warning("Invalid vendor ID: {VendorId}", request.VendorId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, VendorConstants.INVALID_VENDOR_ID);
                }

                var vendor = await _vendorDataService.GetVendorByIdAsync(request.VendorId);
                if (vendor == null)
                {
                    Log.Warning("Vendor not found: ID {VendorId}", request.VendorId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, VendorConstants.VENDOR_NOT_FOUND);
                }

                vendor.IsDeleted = true;
                vendor.UpdatedBy = userId;
                vendor.UpdatedAt = DateTime.Now;

                var result = await _vendorDataService.UpdateVendorAsync(vendor);
                if (!result)
                {
                    Log.Error("Failed to delete vendor: ID {VendorId}", request.VendorId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, VendorConstants.FAILED_TO_DELETE_VENDOR);
                }

                Log.Information("Vendor deleted successfully: ID {VendorId}", request.VendorId);
                return ApiResponse<string>.CreateResponse(ResponseCode.Ok, VendorConstants.VENDOR_DELETED_SUCCESSFULLY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting vendor: ID {VendorId}", request.VendorId);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"{VendorConstants.ERROR_DELETING_VENDOR}: {ex.Message}");
            }
        }
        /// <summary>
        /// Fetches all vendors from the system.
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<VendorDetails>>> GetAllVendorsAsync()
        {
            try
            {
                Log.Information("Fetching all vendors");
                var vendors = await _vendorDataService.GetAllVendorsAsync();
                Log.Information("Fetched {Count} vendors", vendors.Count);
                return ApiResponse<List<VendorDetails>>.CreateResponse(ResponseCode.Ok, "", vendors);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving vendors");
                return ApiResponse<List<VendorDetails>>.CreateResponse(ResponseCode.InternalServerError, $"{VendorConstants.ERROR_RETRIEVING_VENDORS}: {ex.Message}");
            }
        }
        /// <summary>
        /// Updates an existing vendor in the system.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> UpdateVendorAsync(UpdateVendorRequest request, long userId)
        {
            try
            {
                Log.Information("Updating vendor: ID {VendorId}", request.VendorId);

                var vendor = await _vendorDataService.GetVendorByIdAsync(request.VendorId);
                if (vendor == null || vendor.IsDeleted)
                {
                    Log.Warning("Vendor not found or deleted: ID {VendorId}", request.VendorId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, VendorConstants.VENDOR_NOT_FOUND);
                }

                if (request.Name != null) vendor.Name = request.Name;
                if (request.StoreName != null) vendor.StoreName = request.StoreName;
                if (request.Phone != null) vendor.Phone = request.Phone;
                if (request.Email != null) vendor.Email = request.Email;
                if (request.Address != null) vendor.Address = request.Address;
                if (request.City != null) vendor.City = request.City;
                if (request.State != null) vendor.State = request.State;
                if (request.Country != null) vendor.Country = request.Country;
                if (request.Pincode != null) vendor.Pincode = request.Pincode;

                vendor.UpdatedBy = userId;
                vendor.UpdatedAt = DateTime.Now;

                await _vendorDataService.UpdateVendorAsync(vendor);

                Log.Information("Vendor updated successfully: ID {VendorId}", request.VendorId);
                return ApiResponse<string>.CreateResponse(ResponseCode.Ok, VendorConstants.VENDOR_UPDATED_SUCCESSFULLY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating vendor: ID {VendorId}", request.VendorId);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"{VendorConstants.ERROR_UPDATING_VENDOR}: {ex.Message}");
            }
        }
    }
}