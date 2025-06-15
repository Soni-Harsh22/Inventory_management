namespace InventoryManagementSystem.Common.Constants
{
    public static class ServiceMessageConstants
    {
        // OrderService - PlaceCustomerOrderAsync
        public const string InvalidOrInsufficientStock = "Invalid or insufficient stock for product {0}";
        public const string OrderPlacedSuccessfully = "Customer order placed successfully.";
        public const string ErrorPlacingOrder = "An error occurred while placing the order.";

        // OrderService - ReturnMultipleOrderItemsAsync
        public const string NoMatchingOrderItemsFound = "No matching order items found.";
        public const string ReturnSuccess = "Selected order items returned successfully.";
        public const string ReturnFailed = "Failed to return items.";
        public const string ErrorProcessingReturn = "An error occurred while processing the return.";
        public const string OrderALreadyReturn = "\"ProductItem {0} in OrderId {1} has already been returned\"";

        // OrderService - GetCustomerOrderByIdAsync
        public const string OrderNotFound = "Order not found.";
        public const string ErrorFetchingOrderDetails = "An error occurred while fetching order details.";

        // OrderService - GetCustomerOrdersByDateRangeAsync
        public const string InvalidMonth = "Invalid month(s).";
        public const string FutureMonth = "Month cannot be in the future.";
        public const string FutureYear = "Year cannot be in the future.";
        public const string INVALID_DATE_RANGE = "Start date must be before end date.";
        public const string ERRORFETCHINGORDERS = "An error occurred while fetching orders.";
        public static string ORDER_NOT_FOUND_FOR_DATES = "No orders found between the specified dates";

    }
}
