using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;
using PDAB.Services;

namespace PDAB.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IDialogService _dialogService;
        private readonly PasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly InvoiceService _invoiceService;
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
                    var deleteMethod = ActiveWorkspace.GetType().GetMethod("DeleteSelectedItem");
                    await (Task)deleteMethod.Invoke(ActiveWorkspace, null);
                }
                catch
                {
                    _deleteCommand.RaiseCanExecuteChanged();
                    throw;
                }
            },
            canExecute: () => 
            {
                if (ActiveWorkspace == null) return false;

                var type = ActiveWorkspace.GetType();
                var isDataView = type.BaseType?.IsGenericType == true &&
                                 type.BaseType.GetGenericTypeDefinition() == typeof(BaseDataViewModel<>);
        
                var selectedItemProperty = isDataView ? type.GetProperty("SelectedItem") : null;

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
        public ICommand ShowDashboardCommand => new BaseCommand(() => ShowDashboard());
        public ICommand ShowVisualOrderCommand => new BaseCommand(() => ShowVisualOrder());
        public ICommand ShowOrderInvoicesCommand => new BaseCommand(() => ShowOrderInvoice());


        # endregion
        
        public MainWindowViewModel(
            IRepositoryFactory repositoryFactory,
            IDialogService dialogService, PasswordService passwordService, IEmailService emailService,InvoiceService invoiceService)
        {
            DisplayName = "PDAB Marcin Binkowski";
            _repositoryFactory = repositoryFactory;
            _dialogService = dialogService;
            _passwordService = passwordService;
            _emailService = emailService;
            _invoiceService = invoiceService;

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
        
        private void ShowDashboard() =>
            AddWorkspace(new DashboardViewModel(_repositoryFactory));        
        private void ShowVisualOrder() =>
            AddWorkspace(new VisualOrderViewModel(_repositoryFactory, _emailService));
        
        private void ShowOrderInvoice() =>
            AddWorkspace(new OrderInvoiceViewModel(_repositoryFactory,_invoiceService, _emailService));
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
                BaseDataViewModel<Category> =>
                    new AddCategoryViewModel(_repositoryFactory.GetRepository<Category>()),
                BaseDataViewModel<Customer> =>
                    new AddCustomerViewModel(_repositoryFactory.GetRepository<Customer>()),
                BaseDataViewModel<DiscountProduct> => 
                    new AddDiscountProductViewModel(
                        _repositoryFactory,
                        _dialogService
                    ),
                BaseDataViewModel<Discount> =>
                    new AddDiscountViewModel(_repositoryFactory.GetRepository<Discount>()),
                BaseDataViewModel<Employee> =>
                    new AddEmployeeViewModel(_repositoryFactory.GetRepository<Employee>()),
                BaseDataViewModel<Manufacturer> =>
                    new AddManufacturerViewModel(_repositoryFactory.GetRepository<Manufacturer>()),
                BaseDataViewModel<OrderDetail> =>
                    new AddOrderDetailViewModel(
                        _repositoryFactory,
                        _dialogService),
                BaseDataViewModel<OrderPayment> =>
                    new AddOrderPaymentViewModel(
                        _repositoryFactory,
                        _dialogService),
                BaseDataViewModel<OrderStatus> =>
                    new AddOrderStatusViewModel(_repositoryFactory.GetRepository<OrderStatus>()),
                BaseDataViewModel<Order> =>
                    new AddOrderViewModel(
                        _repositoryFactory,
                        _dialogService
                        ),
                BaseDataViewModel<PaymentMethod> =>
                    new AddPaymentMethodViewModel(_repositoryFactory.GetRepository<PaymentMethod>()),
                BaseDataViewModel<ProductImage> =>
                    new AddProductImageViewModel( 
                        _repositoryFactory,
                        _dialogService),
                BaseDataViewModel<Product> =>
                    new AddProductViewModel(           
                        _repositoryFactory,
                        _dialogService),
                BaseDataViewModel<Review> =>
                    new AddReviewViewModel(
                        _repositoryFactory,
                        _dialogService),
                BaseDataViewModel<Role> =>
                    new AddRoleViewModel(_repositoryFactory.GetRepository<Role>()),
                BaseDataViewModel<User> =>
                    new AddUserViewModel(   
                        _repositoryFactory,
                        _dialogService,
                        _passwordService
                        ),
                _ => null
            };

            if (viewModel != null)
            {
                _previousWorkspace = ActiveWorkspace;
                viewModel.RequestClose += async (s, e) =>
                {
                    Workspaces.Remove(viewModel);
                    if (_previousWorkspace != null)
                    {
                        await (_previousWorkspace as IRefreshable)?.RefreshAsync();
                        Workspaces.Add(_previousWorkspace);
                        ActiveWorkspace = _previousWorkspace;
                    }
                };
                Workspaces.Clear();
                Workspaces.Add(viewModel);
                ActiveWorkspace = viewModel;
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