using Azure;
using InventoryManagementSystem.BussinessService.Implemantations;
using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route(ApiEndPoints.API_CONTROLLER)]
    [ApiController]
    public class VendorController : BaseController
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPost(ApiEndPoints.ADD_VENDOR)]
        public async Task<IActionResult> AddVendor([FromBody] VendorRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _vendorService.AddVendorAsync(request, userId);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPatch(ApiEndPoints.DELETE_VENDOR)]
        public async Task<IActionResult> DeleteVendor([FromBody] DeleteVendorRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _vendorService.DeleteVendorAsync(request, userId);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.EMPLOYEES)]
        [HttpGet(ApiEndPoints.GET_ALL_VENDORS)]
        public async Task<IActionResult> GetAllVendors()
        {
            var response = await _vendorService.GetAllVendorsAsync();
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPut(ApiEndPoints.UPDATE_VENDOR)]
        public async Task<IActionResult> UpdateVendor([FromBody] UpdateVendorRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _vendorService.UpdateVendorAsync(request, userId);
            return GenerateResponse(response);
        }

    }
}