namespace InventoryManagementSystem.Common.Constants
{
    public static class ProductServiceResponseMessages
    {
        public const string PRODUCT_NAME_REQUIRED = "Product name is required.";
        public const string VALID_CATEGORY_ID_REQUIRED = "Valid CategoryId is required.";
        public const string CATEGORY_NOT_FOUND = "Category not found.";
        public const string PRODUCT_ADDED_SUCCESSFULLY = "Product added successfully. ID: {0}";
        public const string ERROR_WHILE_ADDING_PRODUCT = "Error while adding product";
        public const string INVALID_PRODUCT_ID = "Invalid product ID.";
        public const string PRODUCT_NOT_FOUND = "Product not found.";
        public const string PRODUCT_DELETED_SUCCESSFULLY = "Product deleted successfully.";
        public const string FAILED_TO_DELETE_PRODUCT = "Failed to delete product.";
        public const string ERROR_WHILE_DELETING_PRODUCT = "Error while soft deleting product";
        public const string PRODUCT_STATUS_UPDATED = "Product status updated successfully.";
        public const string FAILED_TO_UPDATE_PRODUCT_STATUS = "Failed to update product status.";
        public const string ERROR_WHILE_UPDATING_PRODUCT_STATUS = "Error while updating product status";
        public const string PRODUCT_UPDATED_SUCCESSFULLY = "Product updated successfully";
        public const string ERROR_UPDATING_PRODUCT = "Error updating product";
        public const string ERROR_FETCHING_PRODUCTS = "Error fetching all products";
        public const string ERROR_FETCHING_LOW_STOCK_PRODUCTS = "Error fetching low stock products";
        public const string ERROR_SEARCHING_PRODUCTS = "Error searching products with keyword: {0}";
        public const string NO_LOW_STOCK_PRODUCTES = "No low stock productes are present in inventory";
        public const string PRODUCT_FETCH_SUCESS = "Product fetch sucessfully";

    }
}
