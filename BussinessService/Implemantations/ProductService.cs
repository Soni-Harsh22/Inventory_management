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
    public class ProductService : IProductService
    {
        private readonly IProductDataService _productDataService;
        private readonly IStocksMovementService _stocksMovementService;
        private readonly ICategoryDataService _categoryDataService;

        public ProductService(IProductDataService productDataService, IStocksMovementService stocksMovementService, ICategoryDataService categoryDataService)
        {
            _productDataService = productDataService;
            _stocksMovementService = stocksMovementService;
            _categoryDataService = categoryDataService;
        }
        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> AddProductAsync(AddProductRequest request, long userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, ProductServiceResponseMessages.PRODUCT_NAME_REQUIRED);

                if (request.CategoryId <= 0)
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, ProductServiceResponseMessages.VALID_CATEGORY_ID_REQUIRED);

                var categoryExists = await _categoryDataService.GetCategoryByIdAsync(request.CategoryId);
                if (categoryExists == null)
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.CATEGORY_NOT_FOUND);

                string formattedName = char.ToUpper(request.Name.Trim()[0]) + request.Name.Trim().Substring(1).ToLower();

                var product = new Products
                {
                    Name = formattedName,
                    Description = request.Description,
                    CategoryId = request.CategoryId,
                    Price = request.Price,
                    CreatedBy = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedBy = userId,
                    IsDeleted = false,
                    ProductStatus = (int)StockStatusEnum.NewAdded,
                    MinimumRequiredQuantity = request.MinimumRequiredQuantity
                };

                await _productDataService.AddProductAsync(product);
                await _stocksMovementService.StockMovementAsync(product.ProductId, userId, request.Comment!, 0, OrderStatusEnum.Pending);

                Log.Information("Product added successfully: {@Product}", product);

                return ApiResponse<string>.CreateResponse(ResponseCode.Created, string.Format(ProductServiceResponseMessages.PRODUCT_ADDED_SUCCESSFULLY, product.ProductId));
            }
            catch (Exception ex)
            {
                Log.Error(ex, ProductServiceResponseMessages.ERROR_WHILE_ADDING_PRODUCT);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// Soft deletes a product from the system.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> SoftDeleteProductAsync(DeleteProductRequest request, long userId)
        {
            try
            {
                if (request.ProductId <= 0)
                    return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, ProductServiceResponseMessages.INVALID_PRODUCT_ID);

                var product = await _productDataService.GetProductByIdAsync(request.ProductId);
                if (product == null)
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.PRODUCT_NOT_FOUND);

                product.UpdatedBy = userId;
                product.UpdatedAt = DateTime.Now;
                product.IsDeleted = true;

                var result = await _productDataService.UpdateProductAsync(product);

                if (result)
                {
                    Log.Information("Product deleted successfully. ID: {ProductId}", request.ProductId);
                    return ApiResponse<string>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_DELETED_SUCCESSFULLY);
                }

                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, ProductServiceResponseMessages.FAILED_TO_DELETE_PRODUCT);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ProductServiceResponseMessages.ERROR_WHILE_DELETING_PRODUCT);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Updates the details of an existing product.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> UpdateProductAsync(UpdateProductRequest request, long userId)
        {
            try
            {
                var product = await _productDataService.GetProductByIdAsync(request.ProductId);
                if (product == null)
                    return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.PRODUCT_NOT_FOUND);

                if (request.CategoryId.HasValue)
                {
                    var categoryExists = await _categoryDataService.GetCategoryByIdAsync(request.CategoryId.Value);
                    if (categoryExists == null)
                        return ApiResponse<string>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.CATEGORY_NOT_FOUND);
                }

                if (!string.IsNullOrWhiteSpace(request.Name)) product.Name = request.Name;
                if (!string.IsNullOrWhiteSpace(request.Description)) product.Description = request.Description;
                if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId.Value;
                if (request.Price.HasValue) product.Price = request.Price.Value;
                if (request.MinimumRequiredQuantity.HasValue) product.MinimumRequiredQuantity = request.MinimumRequiredQuantity.Value;

                product.UpdatedBy = userId;
                product.UpdatedAt = DateTime.Now;

                await _productDataService.UpdateProductAsync(product);

                Log.Information("Product updated successfully. ID: {ProductId}", request.ProductId);
                return ApiResponse<string>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_UPDATED_SUCCESSFULLY);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ProductServiceResponseMessages.ERROR_UPDATING_PRODUCT);
                return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, $"Error updating product: {ex.Message}");
            }
        }
        /// <summary>
        /// Get all products from the system.
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<ProductResponse>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productDataService.GetAllProductsAsync();
                if (products.Count == 0)
                {
                    return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.PRODUCT_NOT_FOUND);
                }

                var result = products.Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Price = p.Price,
                    AvailableQuantity = p.AvailableQuantity,
                    ProductStatus = ((StockStatusEnum)p.ProductStatus).ToString()
                }).ToList();

                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_FETCH_SUCESS, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ProductServiceResponseMessages.ERROR_FETCHING_PRODUCTS);
                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.InternalServerError, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Get all products with low stock from the system.
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<ProductResponse>>> GetLowStockProductsAsync()
        {
            try
            {
                var products = await _productDataService.GetLowStockProductsAsync();
                if (products.Count == 0)
                {
                    return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.NO_LOW_STOCK_PRODUCTES);
                }

                var result = products.Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Price = p.Price,
                    AvailableQuantity = p.AvailableQuantity,
                    ProductStatus = ((StockStatusEnum)p.ProductStatus).ToString()
                }).ToList();

                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_FETCH_SUCESS, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ProductServiceResponseMessages.ERROR_FETCHING_LOW_STOCK_PRODUCTS);
                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.InternalServerError, $"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Search for products by name.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ProductResponse>>> SearchProductsAsync(string keyword)
        {
            try
            {
                var products = await _productDataService.SearchProductsByNameAsync(keyword);
                if (products.Count == 0)
                {
                    return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.PRODUCT_NOT_FOUND);
                }

                var result = products.Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    AvailableQuantity = p.AvailableQuantity,
                    ProductStatus = ((StockStatusEnum)p.ProductStatus).ToString()
                }).ToList();

                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_FETCH_SUCESS, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Format(ProductServiceResponseMessages.ERROR_SEARCHING_PRODUCTS, keyword));
                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.InternalServerError, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<ProductResponse>>> GetProductByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _productDataService.GetProductByCategoryAsync(categoryId);
                if (products.Count == 0)
                {
                    return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.PRODUCT_NOT_FOUND);
                }

                var result = products.Select(p => new ProductResponse
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Price = p.Price,
                    AvailableQuantity = p.AvailableQuantity,
                    ProductStatus = ((StockStatusEnum)p.ProductStatus).ToString()
                }).ToList();

                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_FETCH_SUCESS, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Format(ProductServiceResponseMessages.ERROR_SEARCHING_PRODUCTS, categoryId));
                return ApiResponse<List<ProductResponse>>.CreateResponse(ResponseCode.InternalServerError, $"Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ProductResponse>> GetProductByIdAsync(long productId)
        {
            try
            {
                var products = await _productDataService.GetProductByIdAsync(productId);
                if (products == null)
                {
                    return ApiResponse<ProductResponse>.CreateResponse(ResponseCode.NotFound, ProductServiceResponseMessages.PRODUCT_NOT_FOUND);
                }

                var result = new ProductResponse
                {
                    ProductId = products.ProductId,
                    Name = products.Name,
                    Description = products.Description,
                    CategoryId = products.CategoryId,
                    Price = products.Price,
                    AvailableQuantity = products.AvailableQuantity,
                    ProductStatus = ((StockStatusEnum)products.ProductStatus).ToString()
                };

                return ApiResponse<ProductResponse>.CreateResponse(ResponseCode.Ok, ProductServiceResponseMessages.PRODUCT_FETCH_SUCESS, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Format(ProductServiceResponseMessages.ERROR_SEARCHING_PRODUCTS, productId));
                return ApiResponse<ProductResponse>.CreateResponse(ResponseCode.InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}