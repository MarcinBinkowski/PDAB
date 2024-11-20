using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;
using PDAB.Repository;
using PDAB.Services;
using PDAB.Helpers;
using PDAB.Views;

namespace PDAB.ViewModels
{
    public class OrderInvoiceViewModel : BaseWorkspaceViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly InvoiceService _invoiceService;
        private readonly IEmailService _emailService;
        private ObservableCollection<Customer> _customers;
        private Customer _selectedCustomer;
        private ObservableCollection<Order> _customerOrders;
        private Order _selectedOrder;
        private byte[] _currentPdfContent;
        private BaseCommand _saveInvoiceCommand;
        private BaseCommand _sendInvoiceCommand;
        private BaseCommand _previewInvoiceCommand;

        public OrderInvoiceViewModel(IRepositoryFactory repositoryFactory, 
            InvoiceService invoiceService, 
            IEmailService emailService)
        {
            DisplayName = "Order Invoices";
            _repositoryFactory = repositoryFactory;
            _invoiceService = invoiceService;
            _emailService = emailService;
            LoadCustomers();
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

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                LoadCustomerOrders();
            }
        }

        public ObservableCollection<Order> CustomerOrders
        {
            get => _customerOrders;
            set
            {
                _customerOrders = value;
                OnPropertyChanged(nameof(CustomerOrders));
            }
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                Console.WriteLine(
                    $"SelectedOrder: {_selectedOrder?.OrderId} " +
                    $"SelectedOrder.OrderDetails: {_selectedOrder?.OrderDetails?.Count}"
                );                
                if (_selectedOrder != null)
                {
                    LoadOrderDetails(_selectedOrder);
                }
                OnPropertyChanged(nameof(SelectedOrder));
                (_saveInvoiceCommand as BaseCommand)?.RaiseCanExecuteChanged();
                (_sendInvoiceCommand as BaseCommand)?.RaiseCanExecuteChanged();
                (_previewInvoiceCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveInvoiceCommand => _saveInvoiceCommand ??= new BaseCommand(
            SaveInvoice, 
            () => SelectedOrder != null
        );

        public ICommand SendInvoiceCommand => _sendInvoiceCommand ??= new BaseCommand(
            SendInvoice, 
            () => SelectedOrder != null
        );

        public ICommand PreviewInvoiceCommand => _previewInvoiceCommand ??= new BaseCommand(
            PreviewInvoice, 
            () => SelectedOrder != null
        );

        private async void LoadCustomers()
        {
            try
            {
                var customers = await _repositoryFactory.GetRepository<Customer>().GetAllAsync();
                Customers = new ObservableCollection<Customer>(
                    customers.OrderBy(c => c.LastName)
                );
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error loading customers: {ex.Message}", MessageBoxImage.Error);
            }
        }

        private async void LoadCustomerOrders()
        {
            if (SelectedCustomer == null)
            {
                CustomerOrders = new ObservableCollection<Order>();
                return;
            }

            try
            {
                var orders = await _repositoryFactory.GetRepository<Order>().GetAllIncludingAsync(
                    query => query
                        .Include(o => o.OrderStatus)
                        .Include(o => o.Customer)
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Product)
                        .Where(o => o.CustomerId == SelectedCustomer.CustomerId)
                        .OrderByDescending(o => o.OrderDate)
                );
                
                CustomerOrders = new ObservableCollection<Order>(orders);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error loading orders: {ex.Message}", MessageBoxImage.Error);
            }
        }

        private async void LoadOrderDetails(Order order)
        {
            try
            {
                var repository = _repositoryFactory.GetRepository<Order>();
                var orders = await repository.GetAllIncludingAsync(
                    query => query
                        .Include(o => o.Customer)
                        .Include(o => o.OrderStatus)
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                );

                var orderWithDetails = orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                if (orderWithDetails != null)
                {
                    var updatedOrders = CustomerOrders.ToList();
                    var index = updatedOrders.FindIndex(o => o.OrderId == order.OrderId);
                    updatedOrders[index] = orderWithDetails;
            
                    CustomerOrders = new ObservableCollection<Order>(updatedOrders);
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error loading order details: {ex.Message}", MessageBoxImage.Error);
            }
        }


        private void SaveInvoice()
        {
            try
            {
                _currentPdfContent = _invoiceService.GenerateInvoice(SelectedOrder);
                var dialog = new SaveFileDialog
                {
                    FileName = $"Invoice_{SelectedOrder.OrderId}.pdf",
                    DefaultExt = ".pdf",
                    Filter = "PDF documents (.pdf)|*.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(dialog.FileName, _currentPdfContent);
                    ShowMessageBox("Invoice saved successfully!", MessageBoxImage.Information);
                    ResetView();
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error saving invoice: {ex.Message}", MessageBoxImage.Error);
            }
        }

        private async void SendInvoice()
        {
            var orderToProcess = SelectedOrder;
            if (orderToProcess == null) return;

            try
            {
                _currentPdfContent = _invoiceService.GenerateInvoice(orderToProcess);
                Console.WriteLine($"Sending invoice for order {orderToProcess.OrderId} to {orderToProcess.Customer.Email}");
                await _emailService.SendInvoiceEmailAsync(
                    orderToProcess.Customer.Email,
                    orderToProcess.OrderId,
                    _currentPdfContent
                );

                ShowMessageBox("Invoice sent successfully!", MessageBoxImage.Information);
                ResetView();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error sending invoice: {ex.Message}", MessageBoxImage.Error);
            }
        }

        private void PreviewInvoice()
        {
            var html = _invoiceService.GenerateHtmlInvoice(SelectedOrder);
            var previewWindow = new InvoiceHtmlPreviewWindow(html);
            previewWindow.ShowDialog();
        }

        private void ResetView()
        {
            SelectedCustomer = null;
            SelectedOrder = null;
            CustomerOrders = new ObservableCollection<Order>();
            _currentPdfContent = null;
        }

        public byte[] GetCurrentPdfContent()
        {
            return _currentPdfContent;
        }
    }
}