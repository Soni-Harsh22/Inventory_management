using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Common.Constants
{
    public class ApiEndPoints
    {
        public const string API_CONTROLLER = "api/[controller]";
        //Authentication endpoints
        public const string REGISTER = "register";
        public const string LOGIN = "login";

        //Category endpoints
        public const string ADD_CATEGORY = "add-category";
        public const string GET_ALL_CATEGORY = "get-all-category";
        public const string DELETE_CATEGORY = "delete";
        public const string UPDATE_CATEGORY_STATUS = "update-status";

        //Customer order endpoints
        public const string CUSTOMER_ORDER = "customer-order";
        public const string RETURN_ORDER_ITEMS = "return-order-items";
        public const string GET_CUSTOMER_ORDER_BY_ID = "customer-order/{orderId}";
        public const string GET_CUSTOMER_ORDERS_BETWEEN_DATES = "customer-orders-between-months";

        //Product endpoints
        public const string ADD_PRODUCT = "add-product";
        public const string DELETE_PRODUCT = "delete-product";
        public const string UPDATE_PRODUCT = "update-product";
        public const string GET_ALL_PRODUCTS = "get-all-products";
        public const string SEARCH_PRODUCTS = "search-products";
        public const string SEARCH_BY_CATEGORY = "search-by-category";
        public const string SEARCH_BY_PRODUCTID = "search-by-productid";
        public const string LOW_STOCK_PRODUCT = "get-low-stock-product";


        //Stock order endpoints
        public const string PURCHASE_ORDER = "purchase-order";
        public const string GET_PURCHASE_ORDER_BY_ID = "{orderId}";
        public const string FILTER_BY_DATES = "filter-by-dates";
        public const string UPDATE_STOCK_ORDER_STATUS = "update-stock-order-status";

        //Vendor endpoints
        public const string ADD_VENDOR = "add-vendor";
        public const string DELETE_VENDOR = "delete-vendor";
        public const string GET_ALL_VENDORS = "get-all-vendors";
        public const string UPDATE_VENDOR = "update-vendor-details";
    }
}