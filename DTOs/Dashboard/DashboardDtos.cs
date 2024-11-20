namespace PDAB.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ProductSalesDto> TopProducts { get; set; }
        public List<MonthlyChartDto> MonthlySales { get; set; }
    }

    public class ProductSalesDto
    {
        public string ProductName { get; set; }
        public decimal TotalSales { get; set; }
        public int SoldQuantity { get; set; }
    }

    public class MonthlyChartDto
    {
        public string MonthYear { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}