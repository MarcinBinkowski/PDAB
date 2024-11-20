namespace PDAB.DTOs.Invoice
{
    public class InvoiceDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalAmount { get; set; }
        public CustomerInvoiceDto Customer { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
    }

    public class CustomerInvoiceDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class InvoiceItemDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}