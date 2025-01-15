using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private ObservableCollection<BaseWorkspaceViewModel?> _workspaces;
        private BaseWorkspaceViewModel _activeWorkspace;
        private BaseWorkspaceViewModel _previousWorkspace;
        private BaseCommand _refreshCommand;
        private BaseCommand _deleteCommand;


        public ICommand AddCommand => new BaseCommand(AddNew);
        public ICommand RefreshCommand => _refreshCommand ??= new BaseCommand(
            execute: async () => await RefreshActiveWorkspace(),
            canExecute: () => CanRefresh()
        );

        public ICommand DeleteCommand => _deleteCommand ??= new BaseCommand(
            execute: async () => 
            {
                try
                {
                    if (ActiveWorkspace?.GetType().BaseType?.GetGenericTypeDefinition() == typeof(BaseDataViewModel<>))
                    {
                        var deleteMethod = ActiveWorkspace.GetType().GetMethod("DeleteSelectedItem");
                        if (deleteMethod != null)
                        {
                            await (Task)deleteMethod.Invoke(ActiveWorkspace, null);
                        }
                    }
                }
                catch
                {
                    _deleteCommand.RaiseCanExecuteChanged();
                    throw;
                }
            },
            canExecute: () => 
            {
                var isDataView = ActiveWorkspace?.GetType().BaseType?.GetGenericTypeDefinition() == typeof(BaseDataViewModel<>);
                Console.WriteLine($"CanDelete called, result: {isDataView}, ActiveWorkspace: {ActiveWorkspace?.GetType().Name}, BaseType: {ActiveWorkspace?.GetType().BaseType?.Name}");
                return isDataView;
            }
           
        );


        public ObservableCollection<BaseWorkspaceViewModel?> Workspaces
        {
            get => _workspaces;
            set
            {
                _workspaces = value;
                OnPropertyChanged(nameof(Workspaces));
            }
        }
        public BaseWorkspaceViewModel ActiveWorkspace
        {
            get => _activeWorkspace;
            set
            {
                _activeWorkspace = value;
                OnPropertyChanged(nameof(ActiveWorkspace));
                _refreshCommand?.RaiseCanExecuteChanged();
                _deleteCommand?.RaiseCanExecuteChanged();
                Console.WriteLine($"ActiveWorkspace changed to: {value?.GetType().Name}");

            }
        }

        
        # region commands
        public ICommand ShowCategoriesCommand => new BaseCommand(() => ShowCategories());
        public ICommand ShowCustomersCommand => new BaseCommand(() => ShowCustomers());
        public ICommand ShowDiscountProductsCommand => new BaseCommand(() => ShowDiscountProducts());
        public ICommand ShowDiscountsCommand => new BaseCommand(() => ShowDiscounts());
        public ICommand ShowEmployeesCommand => new BaseCommand(() => ShowEmployees());
        public ICommand ShowManufacturersCommand => new BaseCommand(() => ShowManufacturers());
        public ICommand ShowOrderDetailsCommand => new BaseCommand(() => ShowOrderDetails());
        public ICommand ShowOrderPaymentsCommand => new BaseCommand(() => ShowOrderPayments());
        public ICommand ShowOrderStatusesCommand => new BaseCommand(() => ShowOrderStatuses());
        public ICommand ShowOrdersCommand => new BaseCommand(() => ShowOrders());
        public ICommand ShowPaymentMethodsCommand => new BaseCommand(() => ShowPaymentMethods());
        public ICommand ShowProductImagesCommand => new BaseCommand(() => ShowProductImages());
        public ICommand ShowProductsCommand => new BaseCommand(() => ShowProducts());
        public ICommand ShowReviewsCommand => new BaseCommand(() => ShowReviews());
        public ICommand ShowRolesCommand => new BaseCommand(() => ShowRoles());
        public ICommand ShowUsersCommand => new BaseCommand(() => ShowUsers());

        # endregion
        
        public MainWindowViewModel(IRepositoryFactory repositoryFactory)
        {
            DisplayName = "PDAB Marcin Binkowski";
            _repositoryFactory = repositoryFactory;
            Workspaces = new ObservableCollection<BaseWorkspaceViewModel?>();

        }
        private void ShowCategories()
        {
            Console.WriteLine("ShowCategories called");
            var viewModel = new AllCategoriesViewModel(_repositoryFactory.GetRepository<Category>());
            AddWorkspace(viewModel);
        }

        private void ShowCustomers()
        {
            var viewModel = new AllCustomersViewModel(_repositoryFactory.GetRepository<Customer>());
            AddWorkspace(viewModel);
        }
        private void ShowDiscountProducts() => 
            AddWorkspace(new AllDiscountProductsViewModel(_repositoryFactory.GetRepository<DiscountProduct>()));

        private void ShowDiscounts() => 
            AddWorkspace(new AllDiscountsViewModel(_repositoryFactory.GetRepository<Discount>()));

        private void ShowEmployees() => 
            AddWorkspace(new AllEmployeesViewModel(_repositoryFactory.GetRepository<Employee>()));

        private void ShowManufacturers() => 
            AddWorkspace(new AllManufacturersViewModel(_repositoryFactory.GetRepository<Manufacturer>()));

        private void ShowOrderDetails() => 
            AddWorkspace(new AllOrderDetailsViewModel(_repositoryFactory.GetRepository<OrderDetail>()));

        private void ShowOrderPayments() => 
            AddWorkspace(new AllOrderPaymentsViewModel(_repositoryFactory.GetRepository<OrderPayment>()));

        private void ShowOrderStatuses() => 
            AddWorkspace(new AllOrderStatusesViewModel(_repositoryFactory.GetRepository<OrderStatus>()));

        private void ShowOrders() => 
            AddWorkspace(new AllOrdersViewModel(_repositoryFactory.GetRepository<Order>()));

        private void ShowPaymentMethods() => 
            AddWorkspace(new AllPaymentMethodsViewModel(_repositoryFactory.GetRepository<PaymentMethod>()));

        private void ShowProductImages() => 
            AddWorkspace(new AllProductImagesViewModel(_repositoryFactory.GetRepository<ProductImage>()));

        private void ShowProducts() => 
            AddWorkspace(new AllProductsViewModel(_repositoryFactory.GetRepository<Product>()));

        private void ShowReviews() => 
            AddWorkspace(new AllReviewsViewModel(_repositoryFactory.GetRepository<Review>()));
        private void ShowRoles() => 
            AddWorkspace(new AllRolesViewModel(_repositoryFactory.GetRepository<Role>()));

        private void ShowUsers() => 
            AddWorkspace(new AllUsersViewModel(_repositoryFactory.GetRepository<User>()));
        
        private void AddWorkspace(BaseWorkspaceViewModel workspace)
        {
            if (workspace is BaseDataViewModel<dynamic> dataView)
            {
                dataView.SelectionChanged += (s, e) => _deleteCommand?.RaiseCanExecuteChanged();
            }
            Workspaces.Clear(); 
            Workspaces.Add(workspace);
            ActiveWorkspace = workspace;
        }
        
        

        private void AddNew()
        {
            BaseWorkspaceViewModel? viewModel = ActiveWorkspace switch
            {
                BaseDataViewModel<Category> categoryView => 
                    new AddCategoryViewModel(_repositoryFactory.GetRepository<Category>()),
                BaseDataViewModel<Customer> customerView => 
                    new AddCustomerViewModel(_repositoryFactory.GetRepository<Customer>()),
                _ => null
            };

            if (viewModel != null)
            {
                _previousWorkspace = ActiveWorkspace;
                viewModel.RequestClose += async (s, e) => 
                {
                    Workspaces.Remove(viewModel);
                    if (_previousWorkspace is BaseDataViewModel<Category> categoryView)
                    {
                        await categoryView.RefreshAsync();
                        Workspaces.Add(categoryView);
                    }
                    else if (_previousWorkspace is BaseDataViewModel<Customer> customerView)
                    {
                        await customerView.RefreshAsync();
                        Workspaces.Add(customerView);
                    }
                };
                Workspaces.Clear();
                Workspaces.Add(viewModel);
            }
        }
        private bool CanRefresh()
        {
            Console.WriteLine("CanRefresh called");
            return ActiveWorkspace is IRefreshable;
        }
        private async Task RefreshActiveWorkspace()
        {
            Console.WriteLine("RefreshActiveWorkspace called");
            if (ActiveWorkspace is IRefreshable refreshable)
            {
                await refreshable.RefreshAsync();
            }
        }
    }
}