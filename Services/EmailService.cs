using System.IO;
using System.Net.Mail;
using System.Windows;

namespace PDAB.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient
            {
                Host = "127.0.0.1",
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public async Task SendOrderConfirmationAsync(string customerEmail, decimal totalAmount)
        {
            Console.WriteLine("Sending order confirmation email...");
            using var message = new MailMessage
            {
                From = new MailAddress("PDAB@test.com"),
                Subject = $"Order Confirmation",
                Body = $@"Dear Customer,
                        Thank you for your order.
                        Total amount: {totalAmount:C}
                        Please check your order status in your account and pay the amount.
                        Best regards,
                        PDAB Sports Store"
            };
            message.To.Add(customerEmail);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendInvoiceEmailAsync(string customerEmail, int orderId, byte[] pdfContent)
        {
            Console.WriteLine($"Sending invoice for order {orderId} to {customerEmail}");
            using var message = new MailMessage
            {
                From = new MailAddress("PDAB@test.com"),
                Subject = $"Invoice nr {orderId}",
                Body = $@"Dear Customer,
Please find attached the invoice for order {orderId}.
Best regards,
PDAB Sports Store",
                Attachments = { new Attachment(new MemoryStream(pdfContent), $"invoice_{orderId}.pdf") }
            };
            message.To.Add(customerEmail);
            await _smtpClient.SendMailAsync(message);
        }
    }
}