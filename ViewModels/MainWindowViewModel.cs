using System.Collections.ObjectModel;
using PDAB.Helpers;
using PDAB.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private BaseViewModel _selectedViewModel;
    private ObservableCollection<BaseWorkspaceViewModel> _workspaces;

    public BaseViewModel SelectedViewModel
    {
        get => _selectedViewModel;
        set
        {
            _selectedViewModel = value;
            OnPropertyChanged(nameof(SelectedViewModel));
        }
    }

    public ObservableCollection<BaseWorkspaceViewModel> Workspaces
    {
        get
        {
            if (_workspaces == null)
                _workspaces = new ObservableCollection<BaseWorkspaceViewModel>();
            return _workspaces;
        }
    }

    public ObservableCollection<CommandViewModel> Commands { get; private set; }

    public MainWindowViewModel()
    {
        Commands = new ObservableCollection<CommandViewModel>
        {
            new CommandViewModel("Orders", new BaseCommand(() => ShowAllOrders())),
            new CommandViewModel("Products", new BaseCommand(() => ShowAllProducts())),
            new CommandViewModel("Customers", new BaseCommand(() => ShowAllCustomers())),
            new CommandViewModel("Employees", new BaseCommand(() => ShowAllEmployees())),
            new CommandViewModel("Roles", new BaseCommand(() => ShowAllRoles())),
            new CommandViewModel("Reviews", new BaseCommand(() => ShowAllReviews())),
            new CommandViewModel("Manufacturers", new BaseCommand(() => ShowAllManufacturers())),
            new CommandViewModel("Categories", new BaseCommand(() => ShowAllCategories())),
            new CommandViewModel("Discount Products", new BaseCommand(() => ShowAllDiscountProducts())),
            new CommandViewModel("Order Statuses", new BaseCommand(() => ShowAllOrderStatus())),
            new CommandViewModel("Payment Methods", new BaseCommand(() => ShowAllPaymentMethods())),
            new CommandViewModel("Product Images", new BaseCommand(() => ShowAllProductImages())),
            new CommandViewModel("Add Order Products", new BaseCommand(() => ShowOrderProductSelection()))
        };
    }

    private void ShowAllOrders()
    {
        ShowWorkspace<AllOrdersViewModel>();
    }

    private void ShowAllProducts()
    {
        ShowWorkspace<AllProductsViewModel>();
    }

    private void ShowAllCustomers()
    {
        ShowWorkspace<AllCustomersViewModel>();
    }

    private void ShowAllEmployees()
    {
        ShowWorkspace<AllEmployeesViewModel>();
    }

    private void ShowAllRoles()
    {
        ShowWorkspace<AllRolesViewModel>();
    }

    private void ShowAllReviews()
    {
        ShowWorkspace<AllReviewsViewModel>();
    }

    private void ShowAllManufacturers()
    {
        ShowWorkspace<AllManufacturersViewModel>();
    }

    private void ShowAllCategories()
    {
        ShowWorkspace<AllCategoriesViewModel>();
    }

    private void ShowAllDiscountProducts()
    {
        ShowWorkspace<AllDiscountProductsViewModel>();
    }

    private void ShowAllOrderStatus()
    {
        ShowWorkspace<AllOrderStatusViewModel>();
    }

    private void ShowAllPaymentMethods()
    {
        ShowWorkspace<AllPaymentMethodsViewModel>();
    }

    private void ShowAllProductImages()
    {
        ShowWorkspace<AllProductImagesViewModel>();
    }

    private void ShowOrderProductSelection()
    {
        ShowWorkspace<OrderProductSelectionViewModel>();
    }

    private void ShowWorkspace<T>() where T : BaseWorkspaceViewModel, new()
    {
        var workspace = Workspaces.FirstOrDefault(vm => vm is T) as T;
        
        if (workspace == null)
        {
            workspace = new T();
            Workspaces.Add(workspace);
        }

        SetActiveWorkspace(workspace);
    }

    private void SetActiveWorkspace(BaseWorkspaceViewModel workspace)
    {
        SelectedViewModel = workspace;
    }
}