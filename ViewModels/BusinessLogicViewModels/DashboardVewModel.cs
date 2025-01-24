using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PDAB.DTOs.Dashboard;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
    public enum ChartType
    {
        Bar,
        Column
    }
    public class DashboardViewModel : BaseWorkspaceViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private DashboardSummaryDto _dashboardSummary;
        private bool _isLoading;
        private int _topCount = 5;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _selectedGroupOption;
        private bool _isTopProductsFiltered;

        public bool IsTopProductsFiltered
        {
            get => _isTopProductsFiltered;
            set
            {
                _isTopProductsFiltered = value;
                OnPropertyChanged(nameof(IsTopProductsFiltered));
                 LoadDashboardData();
            }
        }
        private ChartType _selectedChartType = ChartType.Bar;
        public ChartType SelectedChartType
        {
            get => _selectedChartType;
            set
            {
                if (_selectedChartType != value)
                {
                    Console.WriteLine($"SelectedChartType changing to: {value}");
                    _selectedChartType = value;
                    OnPropertyChanged(nameof(SelectedChartType));
                    LoadDashboardData();
                }
            }
        }

        public DashboardSummaryDto DashboardSummary
        {
            get => _dashboardSummary;
            set
            {
                _dashboardSummary = value;
                OnPropertyChanged(nameof(DashboardSummary));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public int TopCount
        {
            get => _topCount;
            set
            {
                _topCount = value;
                OnPropertyChanged(nameof(TopCount));
                LoadDashboardData();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                LoadDashboardData();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                LoadDashboardData();
            }
        }

        public List<string> GroupOptions { get; } = new() { "Year", "Month", "Week", "Day" };

        public string SelectedGroupOption
        {
            get => _selectedGroupOption;
            set
            {
                _selectedGroupOption = value;
                OnPropertyChanged(nameof(SelectedGroupOption));
                LoadDashboardData();
            }
        }

        public DashboardViewModel(IRepositoryFactory repositoryFactory)
        {
            DisplayName = "Complex Dashboard";
            _repositoryFactory = repositoryFactory;
            _startDate = DateTime.Today.AddMonths(-3);
            _endDate = DateTime.Today;
            _selectedGroupOption = GroupOptions[1]; // Default to "Month"
            _topCount = 10;
        
        LoadDashboardData();
        }

        private async void LoadDashboardData()
        {
            await RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                var orders = await _repositoryFactory.GetRepository<Order>().GetAllAsync();
                var orderDetails = await _repositoryFactory.GetRepository<OrderDetail>().GetAllAsync();
                var products = await _repositoryFactory.GetRepository<Product>().GetAllAsync();

                var filteredOrders = orders.Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate);
                var filteredOrderDetails = orderDetails.Where(od => 
                    filteredOrders.Any(o => o.OrderId == od.OrderId));

                var productQuery = filteredOrderDetails
                    .Join(products,
                        od => od.ProductId,
                        p => p.ProductId,
                        (od, p) => new { OrderDetail = od, Product = p })
                    .GroupBy(x => new { x.Product.ProductId, x.Product.ProductName })
                    .Select(g => new ProductSalesDto
                    {
                        ProductName = g.Key.ProductName,
                        TotalSales = g.Sum(x => x.OrderDetail.Quantity * x.OrderDetail.UnitPrice),
                        SoldQuantity = g.Sum(x => x.OrderDetail.Quantity)
                    })
                    .OrderByDescending(p => p.TotalSales);

                DashboardSummary = new DashboardSummaryDto
                {
                    TotalOrders = filteredOrders.Count(),
                    TotalAmount = filteredOrderDetails.Sum(od => od.Quantity * od.UnitPrice),
                    TopProducts = IsTopProductsFiltered 
                        ? productQuery.Take(TopCount).ToList() 
                        : productQuery.ToList(),
                    MonthlySales = GroupSales(filteredOrders)
                };

                OnPropertyChanged(nameof(DashboardSummary));
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error loading dashboard data: {ex.Message}", MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private List<MonthlyChartDto> GroupSales(IEnumerable<Order> orders)
        {
            return SelectedGroupOption switch
            {
                "Year" => orders
                    .GroupBy(o => o.OrderDate.Year)
                    .Select(g => new MonthlyChartDto
                    {
                        MonthYear = $"Year {g.Key}",
                        OrderCount = g.Count(),
                        TotalAmount = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                    })
                    .OrderBy(x => x.MonthYear)
                    .ToList(),

                "Week" => orders
                    .GroupBy(o => new { o.OrderDate.Year, Week = GetWeekOfYear(o.OrderDate) })
                    .Select(g => new MonthlyChartDto
                    {
                        MonthYear = $"Week {g.Key.Week} of {g.Key.Year}",
                        OrderCount = g.Count(),
                        TotalAmount = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                    })
                    .OrderBy(x => x.MonthYear) 
                    .ToList(),

                "Day" => orders
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new MonthlyChartDto
                    {
                        MonthYear = g.Key.ToShortDateString(),
                        OrderCount = g.Count(),
                        TotalAmount = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                    })
                    .OrderBy(x => x.MonthYear) 
                    .ToList(),

                _ => orders // "Month" default
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Select(g => new MonthlyChartDto
                    {
                        MonthYear = $"{g.Key.Year}-{g.Key.Month:D2}",
                        OrderCount = g.Count(),
                        TotalAmount = g.Sum(o => o.OrderDetails.Sum(od => od.Quantity * od.UnitPrice))
                    })
                    .OrderBy(x => x.MonthYear)
                    .ToList()
            };
        }

        private int GetWeekOfYear(DateTime date)
        {
            var cal = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            return cal.GetWeekOfYear(date, 
                System.Globalization.CalendarWeekRule.FirstFourDayWeek, 
                DayOfWeek.Monday);
        }
    }
}