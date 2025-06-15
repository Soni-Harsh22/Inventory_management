using InventoryManagementSystem.Data.Context;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace InventoryManagementSystem.Common.Helper
{
    public class LowStockCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProductDataService _productDataService;
        private readonly ILogger<LowStockCheckService> _logger;
        private readonly string _adminEmail;
        private readonly TimeSpan _checkInterval;

        /// <summary>
        /// Initializes a new instance of the <see cref="LowStockCheckService"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider for creating scopes.</param>
        /// <param name="logger">Logger for logging information and errors.</param>
        /// <param name="configuration">Configuration for accessing settings.</param>
        public LowStockCheckService(
            IServiceProvider serviceProvider,
            IProductDataService productDataService,
            ILogger<LowStockCheckService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _productDataService = productDataService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _adminEmail = configuration["LowStockNotification:AdminEmail"] ?? throw new ArgumentNullException("AdminEmail not configured");
            _checkInterval = TimeSpan.TryParse(configuration["LowStockNotification:CheckInterval"], out var interval)
                ? interval
                : TimeSpan.FromSeconds(30); 
        }

        /// <summary>
        /// Executes the background service, periodically checking for low-stock products.
        /// </summary>
        /// <param name="stoppingToken">Cancellation token to stop the service.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LowStockCheckService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Checking for low-stock products at {Time}", DateTime.UtcNow);
                    await CheckLowStockProductsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while checking low-stock products.");
                }

                _logger.LogInformation("LowStockCheckService will check again after {Interval}", _checkInterval);
                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("LowStockCheckService stopped.");
        }

        /// <summary>
        /// Checks for products with low stock and sends an email if any are found.
        /// </summary>
        private async Task CheckLowStockProductsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var lowStockProducts = await _productDataService.GetLowStockProductsAsync();

            if (!lowStockProducts.Any())
            {
                _logger.LogInformation("No low-stock products found.");
                return;
            }

            var emailBody = BuildEmailBody(lowStockProducts);
            await emailService.SendEmailAsync(
                to: _adminEmail,
                subject: "Low Stock Alert",
                body: emailBody);

            _logger.LogInformation("Low stock email sent for {Count} products to {AdminEmail}.", lowStockProducts.Count, _adminEmail);
        }

        /// <summary>
        /// Builds the HTML email body for low-stock products.
        /// </summary>
        /// <param name="products">List of low-stock products.</param>
        /// <returns>HTML string for the email body.</returns>
        private string BuildEmailBody(System.Collections.Generic.List<Products> products)
        {
            var sb = new StringBuilder();
            sb.Append("<h2>Low Stock Alert</h2>");
            sb.Append("<p>The following products have an available quantity less than the minimum required quantity:</p>");
            sb.Append("<table border='1' style='border-collapse: collapse; width: 100%;'>");
            sb.Append("<tr style='background-color: #f2f2f2;'>");
            sb.Append("<th style='padding: 8px;'>Product ID</th>");
            sb.Append("<th style='padding: 8px;'>Name</th>");
            sb.Append("<th style='padding: 8px;'>Available Quantity</th>");
            sb.Append("<th style='padding: 8px;'>Minimum Required Quantity</th>");
            sb.Append("</tr>");

            foreach (var product in products)
            {
                sb.Append("<tr>");
                sb.Append($"<td style='padding: 8px;'>{product.ProductId}</td>");
                sb.Append($"<td style='padding: 8px;'>{product.Name}</td>");
                sb.Append($"<td style='padding: 8px;'>{product.AvailableQuantity}</td>");
                sb.Append($"<td style='padding: 8px;'>{product.MinimumRequiredQuantity}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("<p>Please take action to restock these products.</p>");
            return sb.ToString();
        }
    }
}
