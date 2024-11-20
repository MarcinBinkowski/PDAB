using System.IO;
using System.Text;
using Aspose.Pdf;
using PDAB.Models;


namespace PDAB.Services
{
   
    public class InvoiceService
    {
        private readonly string _templatePath;

        public InvoiceService()
        {
            _templatePath = Path.Combine(@"C:\Users\MarcinBinkowski\source\repos\PDAB\Templates\invoice.html");

            if (!File.Exists(_templatePath))
            {
                throw new FileNotFoundException($"Invoice template not found at {_templatePath}");
            }
        }

        public byte[] GenerateInvoice(Order order)
        {
            var template = File.ReadAllText(_templatePath);
            var html = PopulateTemplate(template, order);

            var options = new HtmlLoadOptions();

            using var stream = new MemoryStream();
            using var doc = new Document(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)), options);
            doc.Save(stream);
            return stream.ToArray();
        }

        private string PopulateTemplate(string template, Order order)
        {
            var total = order.OrderDetails.Sum(x => x.Quantity * x.UnitPrice);
        
            var itemsHtml = new StringBuilder();
            foreach (var item in order.OrderDetails)
            {
                itemsHtml.AppendLine($@"
                <tr>
                    <td>{item.Product.ProductName}</td>
                    <td>{item.Quantity}</td>
                    <td>{item.UnitPrice:C}</td>
                    <td>{item.Quantity * item.UnitPrice:C}</td>
                </tr>");
            }

            return template
                .Replace("{{OrderId}}", order.OrderId.ToString())
                .Replace("{{OrderDate}}", order.OrderDate.ToString("d"))
                .Replace("{{CustomerName}}", $"{order.Customer.FirstName} {order.Customer.LastName}")
                .Replace("{{CustomerEmail}}", order.Customer.Email)
                .Replace("{{Items}}", itemsHtml.ToString())
                .Replace("{{Total}}", total.ToString("C"));
        }
        public string GenerateHtmlInvoice(Order order)
        {
            var template = File.ReadAllText(_templatePath);
            return PopulateTemplate(template, order);
        }
    }
}