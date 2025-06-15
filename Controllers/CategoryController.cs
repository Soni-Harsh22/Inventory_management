using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Constants;
using InventoryManagementSystem.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route(ApiEndPoints.API_CONTROLLER)]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _categoryService.AddCategoryAsync(request, userId);
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ALL)]
        [HttpGet(ApiEndPoints.GET_ALL_CATEGORY)]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            return GenerateResponse(response);
        }

        [Authorize(Roles = AccessPermissions.ADMIN)]
        [HttpPatch(ApiEndPoints.DELETE_CATEGORY)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var response = await _categoryService.DeleteCategoryAsync(id, userId);
            return GenerateResponse(response);
        }

    }
}