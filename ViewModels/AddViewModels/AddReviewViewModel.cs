using System.Collections.ObjectModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
public class AddReviewViewModel : BaseAddViewModel<Review>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly DialogService _dialogService;
    private ObservableCollection<Product> _products;
    private ObservableCollection<Customer> _customers;

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

    public AddReviewViewModel(IRepositoryFactory repositoryFactory, DialogService dialogService) 
        : base(repositoryFactory.GetRepository<Review>(), "Add Review")
    {
        _repositoryFactory = repositoryFactory;
        _dialogService = dialogService;
        Entity.ReviewDate = DateTime.Now;
        LoadData();
    }

    private async void LoadData()
    {
        Products = await _repositoryFactory.GetRepository<Product>().GetAllAsync();
        Customers = await _repositoryFactory.GetRepository<Customer>().GetAllAsync();
    }

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

    public ICommand SelectCustomerIdCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Customer", Customers);
        if (selected is Customer customer)
        {
            Entity.CustomerId = customer.CustomerId;
            OnPropertyChanged(nameof(Entity));
        }
    });
}
}