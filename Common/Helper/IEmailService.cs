namespace InventoryManagementSystem.Common.Helper
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
