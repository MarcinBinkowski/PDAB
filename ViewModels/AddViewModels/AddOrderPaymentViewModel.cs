using System.Collections.ObjectModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
public class AddOrderPaymentViewModel : BaseAddViewModel<OrderPayment>
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly DialogService _dialogService;
    private ObservableCollection<Order> _orders;
    private ObservableCollection<PaymentMethod> _paymentMethods;

    public ObservableCollection<Order> Orders
    {
        get => _orders;
        set
        {
            _orders = value;
            OnPropertyChanged(nameof(Orders));
        }
    }

    public ObservableCollection<PaymentMethod> PaymentMethods
    {
        get => _paymentMethods;
        set
        {
            _paymentMethods = value;
            OnPropertyChanged(nameof(PaymentMethods));
        }
    }

    public AddOrderPaymentViewModel(IRepositoryFactory repositoryFactory, DialogService dialogService) 
        : base(repositoryFactory.GetRepository<OrderPayment>(), "Add Order Payment")
    {
        _repositoryFactory = repositoryFactory;
        _dialogService = dialogService;
        Entity.PaymentDate = DateTime.Now;
        LoadData();
    }

    private async void LoadData()
    {
        Orders = await _repositoryFactory.GetRepository<Order>().GetAllAsync();
        PaymentMethods = await _repositoryFactory.GetRepository<PaymentMethod>().GetAllAsync();
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

    public ICommand SelectPaymentMethodCommand => new BaseCommand(async () => 
    {
        var selected = await _dialogService.ShowSelectionDialog("Select Payment Method", PaymentMethods);
        if (selected is PaymentMethod method)
        {
            Entity.PaymentMethod = method;
            Entity.PaymentMethodId = method.PaymentMethodId;
            OnPropertyChanged(nameof(Entity));
        }
    });
}
}