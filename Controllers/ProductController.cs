using Azure;
using InventoryManagementSystem.BussinessService.Implemantations;
using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route(ApiEndPoints.API_CONTROLLER)]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPost(ApiEndPoints.ADD_PRODUCT)]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _productService.AddProductAsync(request, userId);
            return GenerateResponse(response);
        }

        [HttpPatch(ApiEndPoints.DELETE_PRODUCT)]
        [Authorize(Roles = AccessPermissions.ADMIN)]
        public async Task<IActionResult> SoftDeleteProduct([FromBody] DeleteProductRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _productService.SoftDeleteProductAsync(request,userId);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPut(ApiEndPoints.UPDATE_PRODUCT)]
        public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var result = await _productService.UpdateProductAsync(request, userId);
            return GenerateResponse(result);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpGet(ApiEndPoints.GET_ALL_PRODUCTS)]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productService.GetAllProductsAsync();
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpGet(ApiEndPoints.SEARCH_PRODUCTS)]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword)
        {
            var response = await _productService.SearchProductsAsync(keyword);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpGet(ApiEndPoints.SEARCH_BY_CATEGORY)]
        public async Task<IActionResult> SearchProductsByCategory([FromQuery] int categoryId)
        {
            var response = await _productService.GetProductByCategoryAsync(categoryId);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpGet(ApiEndPoints.SEARCH_BY_PRODUCTID)]
        public async Task<IActionResult> SearchProductsById([FromQuery] int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.EMPLOYEES)]
        [HttpGet(ApiEndPoints.LOW_STOCK_PRODUCT)]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var response = await _productService.GetLowStockProductsAsync();
            return GenerateResponse(response);
        }
    }
}