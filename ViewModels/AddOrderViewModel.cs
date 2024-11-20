using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
public class AddOrderViewModel : BaseAddViewModel<Order>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IDialogService _dialogService;
    private ObservableCollection<Customer> _customers;
    private ObservableCollection<OrderStatus> _orderStatuses;

    public ObservableCollection<Customer> Customers
    {
        get => _customers;
        set
        {
            _customers = value;
            OnPropertyChanged(nameof(Customers));
        }
    }

    public ObservableCollection<OrderStatus> OrderStatuses
    {
        get => _orderStatuses;
        set
        {
            _orderStatuses = value;
            OnPropertyChanged(nameof(OrderStatuses));
        }
    }

    public AddOrderViewModel(IRepositoryFactory repositoryFactory, IDialogService dialogService) 
        : base(repositoryFactory.GetRepository<Order>(), "Add Order")
    {
        _repositoryFactory = repositoryFactory;
        _dialogService = dialogService;
        Entity.OrderDate = DateTime.Now;
        Entity.CreatedAt = DateTime.Now;
        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            Customers = await _repositoryFactory.GetRepository<Customer>().GetAllAsync();
            OrderStatuses = await _repositoryFactory.GetRepository<OrderStatus>().GetAllAsync();
        }
        catch (Exception ex)
        {
            ShowMessageBox($"Error loading data: {ex.Message}", MessageBoxImage.Error);
        }
    }

    public ICommand SelectCustomerCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Customer", Customers);
        if (selected is Customer customer)
        {
            Entity.Customer = customer;
            Entity.CustomerId = customer.CustomerId;
            OnPropertyChanged(nameof(Entity));
        }
    });

    public ICommand SelectOrderStatusCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Order Status", OrderStatuses);
        if (selected is OrderStatus status)
        {
            Entity.OrderStatus = status;
            Entity.OrderStatusId = status.OrderStatusId;
            OnPropertyChanged(nameof(Entity));
        }
    });
}

}