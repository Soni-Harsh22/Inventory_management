using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Responces;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Models.Request;
using Serilog;
using InventoryManagementSystem.Common.Constants;

namespace InventoryManagementSystem.BussinessService.Implemantations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDataService _categoryDataService;

        public CategoryService(ICategoryDataService categoryDataService)
        {
            _categoryDataService = categoryDataService;
        }

        /// <summary>
        /// Adds a new category to the system.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> AddCategoryAsync(AddCategoryRequest request, long userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    Log.Warning("AddCategoryAsync failed: Category name is required.");
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, CategoryServiceConstants.CATEGORY_NAME_REQUIRED);
                }

                string formattedName = char.ToUpper(request.Name.Trim()[0]) + request.Name.Trim().Substring(1).ToLower();

                var existingCategory = await _categoryDataService.GetCategoryByNameAsync(formattedName);
                if (existingCategory != null)
                {
                    Log.Warning("AddCategoryAsync failed: Category '{CategoryName}' already exists.", formattedName);
                    return ApiResponse<string>.CreateResponse(ResponseCode.Conflict, CategoryServiceConstants.CATEGORY_ALREADY_EXISTS);
                }

                var category = new Category
                {
                    Name = formattedName,
                    CreatedBy = userId,
                    CreatedAt = DateTime.Now,
                    ActiveStatus = true,
                    IsDeleted = false
                };

                await _categoryDataService.AddCategoryAsync(category);

                Log.Information("Category '{CategoryName}' added successfully by UserId {UserId}.", formattedName, userId);
                return new ApiResponse<string>(ResponseCode.Created, CategoryServiceConstants.CATEGORY_ADD_SUCCESS, formattedName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding category.");
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, CategoryServiceConstants.CATEGORY_ADD_ERROR);
            }
        }

        /// <summary>
        /// Fetches all categories from the system.
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryDataService.GetAllCategoriesAsync();

                if (categories == null || !categories.Any())
                {
                    Log.Information("No categories found.");
                    return ApiResponse<List<Category>>.CreateResponse(ResponseCode.NotFound, CategoryServiceConstants.CATEGORY_NOT_FOUND);
                }

                Log.Information("Fetched {Count} categories.", categories.Count);
                return ApiResponse<List<Category>>.CreateResponse(ResponseCode.Ok, CategoryServiceConstants.CATEGORIES_FETCH_SUCCESS, categories);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching all categories.");
                return ApiResponse<List<Category>>.CreateResponse(ResponseCode.InternalServerError, ErrorCodes.ERROR_OCCURRED);
            }
        }
        /// <summary>
        /// Deletes a category from the system.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> DeleteCategoryAsync(int categoryId, long userId)
        {
            try
            {
                if (categoryId <= 0)
                {
                    Log.Warning("DeleteCategoryAsync failed: Invalid CategoryId {CategoryId}.", categoryId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, CategoryServiceConstants.VALID_CATEGORY_ID_REQUIRED);
                }

                var category = await _categoryDataService.GetCategoryByIdAsync(categoryId);
                if (category == null || category.IsDeleted)
                {
                    Log.Warning("DeleteCategoryAsync failed: CategoryId {CategoryId} not found or already deleted.", categoryId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, CategoryServiceConstants.CATEGORY_NOT_FOUND);
                }

                category.IsDeleted = true;
                category.UpdatedBy = userId;
                category.UpdatedAt = DateTime.Now;

                await _categoryDataService.UpdateCategoryAsync(category);

                Log.Information("CategoryId {CategoryId} deleted by UserId {UserId}.", categoryId, userId);
                return ApiResponse<string>.CreateResponse(ResponseCode.Ok, CategoryServiceConstants.CATEGORY_DELETE_SUCCESS);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting category.");
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, CategoryServiceConstants.CATEGORY_DELETE_ERROR);
            }
        }
    }
}