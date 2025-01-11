using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private BaseViewModel _selectedViewModel;
    private ObservableCollection<BaseWorkspaceViewModel> _workspaces;
    public BaseCommand RefreshCommand { get; private set; }
    public BaseCommand DeleteCommand { get; private set; } 
    public BaseViewModel SelectedViewModel
    {
        get => _selectedViewModel;
        set
        {
            if (_selectedViewModel != null)
            {
                _selectedViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            _selectedViewModel = value;

            if (_selectedViewModel != null)
            {
                _selectedViewModel.PropertyChanged += ViewModel_PropertyChanged;
                Console.WriteLine($"Changed workspace to: {_selectedViewModel.GetType().Name}");
            }

            OnPropertyChanged(nameof(SelectedViewModel));
            UpdateCommandStates();
        }
    }
    private void UpdateCommandStates()
    {
        Console.WriteLine($"Updating command states. CanRefresh: {CanRefresh()}");
        DeleteCommand.RaiseCanExecuteChanged();
        RefreshCommand.RaiseCanExecuteChanged();
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Console.WriteLine($"PropertyChanged: {e.PropertyName} from {sender.GetType().Name}");
        if (e.PropertyName == "SelectedItem")
        {
            Console.WriteLine("Selection changed - updating command states");
            UpdateCommandStates();
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
        RefreshCommand = new BaseCommand(OnRefresh, CanRefresh);
        DeleteCommand = new BaseCommand(OnDelete, CanDelete);

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
            new CommandViewModel("Order Statuses", new BaseCommand(() => ShowAllOrderStatuses())),
            new CommandViewModel("Payment Methods", new BaseCommand(() => ShowAllPaymentMethods())),
            new CommandViewModel("Product Images", new BaseCommand(() => ShowAllProductImages())),
            new CommandViewModel("Add Order Products", new BaseCommand(() => ShowOrderProductSelection()))
        };
    }

    private bool CanRefresh()
    {
        if (SelectedViewModel == null)
        {
            Console.WriteLine("CanRefresh: SelectedViewModel is null");
            return false;
        }

        var viewModelType = SelectedViewModel.GetType();
        Console.WriteLine($"CanRefresh checking type: {viewModelType.Name}");
    
        var isAllEntities = viewModelType.BaseType != null && 
                            viewModelType.BaseType.IsGenericType &&
                            viewModelType.BaseType.GetGenericTypeDefinition() == typeof(AllEntitiesViewModel<>);
    
        Console.WriteLine($"CanRefresh result: {isAllEntities}");
        return isAllEntities;
    }

    private void OnRefresh()
    {
        if (SelectedViewModel is BaseWorkspaceViewModel workspace)
        {
            dynamic vm = workspace;
            vm.Load();
        }
    }

    private bool CanDelete()
    {
        if (SelectedViewModel == null)
        {
            Console.WriteLine("CanDelete: SelectedViewModel is null");
            return false;
        }

        var viewModelType = SelectedViewModel.GetType();
        Console.WriteLine($"CanDelete checking type: {viewModelType.Name}");

        var isAllEntities = viewModelType.BaseType != null && 
                            viewModelType.BaseType.IsGenericType &&
                            viewModelType.BaseType.GetGenericTypeDefinition() == typeof(AllEntitiesViewModel<>);

        if (!isAllEntities)
        {
            Console.WriteLine("CanDelete: Not an AllEntitiesViewModel");
            return false;
        }

        var selectedItemProperty = viewModelType.GetProperty("SelectedItem");
        var selectedItem = selectedItemProperty?.GetValue(SelectedViewModel);
    
        Console.WriteLine($"CanDelete: SelectedItem is {(selectedItem != null ? "not null" : "null")}");
        return selectedItem != null;
    }

    private void OnDelete()
    {
        dynamic vm = SelectedViewModel;
        if (vm.DeleteCommand.CanExecute(null))
        {
            vm.DeleteCommand.Execute(null);
        }
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

    private void ShowAllOrderStatuses()
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
        Console.WriteLine($"Opening workspace of type: {typeof(T).Name}");
        
        var workspace = Workspaces.FirstOrDefault(vm => vm is T) as T;
        
        if (workspace == null)
        {
            workspace = new T();
            Workspaces.Add(workspace);
            Console.WriteLine($"Created new workspace: {workspace.GetType().Name}");
        }
        else
        {
            Console.WriteLine($"Reusing existing workspace: {workspace.GetType().Name}");
        }

        SetActiveWorkspace(workspace);
    }

    private void SetActiveWorkspace(BaseWorkspaceViewModel workspace)
    {
        Console.WriteLine($"Setting active workspace: {workspace.GetType().Name}");
    
        if (_selectedViewModel != null)
        {
            _selectedViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        SelectedViewModel = workspace;
    
        workspace.PropertyChanged += ViewModel_PropertyChanged;
    
        UpdateCommandStates();
    }
}