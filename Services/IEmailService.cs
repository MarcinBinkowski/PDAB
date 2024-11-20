namespace PDAB.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(string customerEmail, decimal totalAmount);
        Task SendInvoiceEmailAsync(string customerEmail, int orderId, byte[] pdfContent);

    }
}