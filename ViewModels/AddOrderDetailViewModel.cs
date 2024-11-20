using System.Collections.ObjectModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
 public class AddOrderDetailViewModel : BaseAddViewModel<OrderDetail>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IDialogService _dialogService;
    private ObservableCollection<Order> _orders;
    private ObservableCollection<Product> _products;
    private ObservableCollection<Discount> _discounts;

    public ObservableCollection<Order> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged(nameof(Orders));
        }
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

    public ObservableCollection<Discount> Discounts
    {
        get => _discounts;
        set
        {
            _discounts = value;
            OnPropertyChanged(nameof(Discounts));
        }
    }

    public AddOrderDetailViewModel(IRepositoryFactory repositoryFactory, IDialogService dialogService) 
        : base(repositoryFactory.GetRepository<OrderDetail>(), "Add Order Detail")
    {
        _repositoryFactory = repositoryFactory;
        _dialogService = dialogService;
        LoadData();
    }

    private async void LoadData()
    {
        Orders = await _repositoryFactory.GetRepository<Order>().GetAllAsync();
        Products = await _repositoryFactory.GetRepository<Product>().GetAllAsync();
        Discounts = await _repositoryFactory.GetRepository<Discount>().GetAllAsync();
    }

    public ICommand SelectOrderCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Order", Orders);
        if (selected is Order order)
        {
            Entity.Order = order;
            Entity.OrderId = order.OrderId;
            OnPropertyChanged(nameof(Entity));
        }
    });

    public ICommand SelectProductCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Product", Products);
        if (selected is Product product)
        {
            Entity.Product = product;
            Entity.ProductId = product.ProductId;
            OnPropertyChanged(nameof(Entity));
        }
    });

    public ICommand SelectDiscountCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Discount", Discounts);
        if (selected is Discount discount)
        {
            Entity.Discount = discount;
            Entity.DiscountId = discount.DiscountId;
            OnPropertyChanged(nameof(Entity));
        }
    });
}
}