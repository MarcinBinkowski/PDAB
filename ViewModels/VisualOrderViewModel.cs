using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;
using PDAB.Services;

namespace PDAB.ViewModels
{
    public class VisualOrderViewModel : BaseWorkspaceViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IEmailService _emailService;

        private ObservableCollection<Product> _products;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<OrderDetail> _orderDetails;

        private Customer _selectedCustomer;
        private string _searchText;
        private decimal _total;

        private BaseCommand _createOrderCommand;

        public VisualOrderViewModel(IRepositoryFactory repositoryFactory, IEmailService emailService)
        {
            DisplayName = "Visual Order Creator";
            _repositoryFactory = repositoryFactory;
            _emailService = emailService;

            // Initialize collections
            _products = new ObservableCollection<Product>();
            _customers = new ObservableCollection<Customer>();
            _orderDetails = new ObservableCollection<OrderDetail>();

            _orderDetails.CollectionChanged += (s, e) =>
            {
                _createOrderCommand?.RaiseCanExecuteChanged();
            };

            _createOrderCommand = new BaseCommand(CreateOrder, CanCreateOrder);

            LoadData();
        }


        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public ObservableCollection<OrderDetail> OrderDetails => _orderDetails;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));

                _createOrderCommand.RaiseCanExecuteChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterProducts();
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        public ICommand CreateOrderCommand => _createOrderCommand;

        public ICommand AddProductCommand => new BaseCommand<Product>(AddProduct);
        public ICommand RemoveProductCommand => new BaseCommand<OrderDetail>(RemoveProduct);
        public ICommand IncreaseQuantityCommand => new BaseCommand<OrderDetail>(IncreaseQuantity);
        public ICommand DecreaseQuantityCommand => new BaseCommand<OrderDetail>(DecreaseQuantity);


        private async void LoadData()
        {
            Products = await _repositoryFactory.GetRepository<Product>().GetAllAsync();
            Customers = await _repositoryFactory.GetRepository<Customer>().GetAllAsync();
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadData();
            }
            else
            {
                var filtered = Products
                    .Where(p => p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                Products = new ObservableCollection<Product>(filtered);
            }
        }


        private void AddProduct(Product product)
        {
            if (product == null) return;

            var existingDetail = _orderDetails.FirstOrDefault(od => od.ProductId == product.ProductId);
            if (existingDetail != null)
            {
                existingDetail.Quantity++;
                var index = _orderDetails.IndexOf(existingDetail);
                _orderDetails.RemoveAt(index);
                _orderDetails.Insert(index, existingDetail);
            }
            else
            {
                _orderDetails.Add(new OrderDetail
                {
                    Product = product,
                    ProductId = product.ProductId,
                    Quantity = 1,
                    UnitPrice = product.UnitPrice
                });
            }

            RecalculateTotal();

            _createOrderCommand.RaiseCanExecuteChanged();
        }

        private void RemoveProduct(OrderDetail detail)
        {
            if (detail == null) return;
            _orderDetails.Remove(detail);

            RecalculateTotal();
            _createOrderCommand.RaiseCanExecuteChanged();
        }

        private void IncreaseQuantity(OrderDetail detail)
        {
            if (detail == null) return;

            detail.Quantity++;
            var index = _orderDetails.IndexOf(detail);
            _orderDetails.RemoveAt(index);
            _orderDetails.Insert(index, detail);

            RecalculateTotal();
            _createOrderCommand.RaiseCanExecuteChanged();
        }

        private void DecreaseQuantity(OrderDetail detail)
        {
            if (detail == null) return;

            if (detail.Quantity <= 1)
            {
                _orderDetails.Remove(detail);
            }
            else
            {
                detail.Quantity--;
                var index = _orderDetails.IndexOf(detail);
                _orderDetails.RemoveAt(index);
                _orderDetails.Insert(index, detail);
            }

            RecalculateTotal();
            _createOrderCommand.RaiseCanExecuteChanged();
        }

        private void RecalculateTotal()
        {
            Total = _orderDetails.Sum(od => od.Quantity * od.UnitPrice);
        }
        
        private bool CanCreateOrder()
        {
            return SelectedCustomer != null && _orderDetails.Any();
        }

        private async void CreateOrder()
        {
            Console.WriteLine("Creating order...");
            Console.WriteLine($"Creating order for customer: {SelectedCustomer.CustomerId}");
            Console.WriteLine($"Order details count: {OrderDetails.Count}");
            var order = new Order
            {
                CustomerId = SelectedCustomer.CustomerId,
                OrderDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                OrderStatusId = 3002
            };
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var repository = _repositoryFactory.GetRepository<Order>();
        
                var savedOrder = await repository.AddAsync(order);
                if (savedOrder.OrderId == 0)
                {
                    throw new Exception("Order was not saved properly - no ID generated");
                }

                var orderDetails = OrderDetails.Select(od => new OrderDetail
                {
                    OrderId = savedOrder.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                }).ToList();
                foreach (var detail in OrderDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice,
                    };
            
                    Console.WriteLine($"Creating order detail: Product={orderDetail.ProductId}, Qty={orderDetail.Quantity}");
                    var orderDetailRepository = _repositoryFactory.GetRepository<OrderDetail>();
                    await orderDetailRepository.AddAsync(orderDetail);
                }
                Console.WriteLine($"Sending Email... {SelectedCustomer.Email}, {Total}");
                transaction.Complete();

                await _emailService.SendOrderConfirmationAsync(
                    SelectedCustomer.Email,
                    Total
                );
                
                OrderDetails.Clear();
                SelectedCustomer = null;
                RecalculateTotal();
                ShowMessageBox("Order created successfully!", MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error creating order: {ex.Message}", MessageBoxImage.Error);
                Console.WriteLine($"ERROR DETAILS: {ex}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }
    }
}
