using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;

namespace PDAB.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
       #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<BaseWorkspaceViewModel> _Workspaces;
        private ObservableCollection<string> _Tables;
        private string _SelectedTable;
        private BaseWorkspaceViewModel _activeWorkspace;
        private ICommand _addNewItemCommand;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Console.WriteLine("Created MainWindowViewModel");
            _Commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands());
            LoadTables();
        }
        #endregion
        
        
        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        
        public BaseWorkspaceViewModel ActiveWorkspace
        {
            get => _activeWorkspace;
            set
            {
                _activeWorkspace = value;
                OnPropertyChanged("c");
                (AddNewItemCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }
        public ICommand AddNewItemCommand
        {
            get
            {
                if (_addNewItemCommand == null)
                {
                    _addNewItemCommand = new BaseCommand(
                        () => AddNewItem(),
                        () => ActiveWorkspace != null);
                }
                return _addNewItemCommand;
            }
        }

        private List<CommandViewModel> CreateCommands()
        {
            Console.WriteLine("CreateCommands called");
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Roles",
                    new BaseCommand(() =>
                    {
                        this.ShowAllRoles();
                    })),
                new CommandViewModel(
                    "Categories",
                    new BaseCommand(() => this.ShowAllCategories())),
                new CommandViewModel("Discounts", new BaseCommand(() => ShowAllDiscounts())),
                new CommandViewModel("Payment Methods", new BaseCommand(() => ShowAllPaymentMethods())),
                new CommandViewModel("Order Status", new BaseCommand(() => ShowAllOrderStatus())),
                new CommandViewModel("Manufacturers", new BaseCommand(() => ShowAllManufacturers())),
                new CommandViewModel("Customers", new BaseCommand(() => ShowAllCustomers())),
                new CommandViewModel("Employees", new BaseCommand(() => ShowAllEmployees())),
                new CommandViewModel("Products", new BaseCommand(() => ShowAllProducts())),
                new CommandViewModel("Orders", new BaseCommand(() => ShowAllOrders())),
                new CommandViewModel("Order Payments", new BaseCommand(() => ShowAllOrderPayments())),
                new CommandViewModel("Reviews", new BaseCommand(() => ShowAllReviews())),
                new CommandViewModel("Users", new BaseCommand(() => ShowAllUsers())),
                new CommandViewModel("Order Details", new BaseCommand(() => ShowAllOrderDetails())),
                new CommandViewModel("Product Images", new BaseCommand(() => ShowAllProductImages())),
                new CommandViewModel("Discount Products", new BaseCommand(() => ShowAllDiscountProducts()))


          };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<BaseWorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<BaseWorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (BaseWorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (BaseWorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            BaseWorkspaceViewModel workspace = sender as BaseWorkspaceViewModel;
            this.Workspaces.Remove(workspace);
        }

        #endregion

        
        
        #region Tables
        public ObservableCollection<string> Tables
        {
            get => _Tables;
            set
            {
                _Tables = value;
                OnPropertyChanged("Tables"); // Use string literal instead of lambda
            }
        }

        public string SelectedTable
        {
            get => _SelectedTable;
            set
            {
                _SelectedTable = value;
                OnPropertyChanged("SelectedTable"); 
            }
        }

        private void LoadTables()
        {
            Tables = new ObservableCollection<string>
            {
                "Roles"
            };
        }
        #endregion
        
        #region ShowTables
        private void ShowAllRoles()
        {
            Console.WriteLine("ShowAllRoles called");
            AllRolesViewModel workspace = 
                this.Workspaces.FirstOrDefault(vm => vm is AllRolesViewModel) 
                    as AllRolesViewModel;
            if (workspace == null)
            {
                workspace = new AllRolesViewModel();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }
        
        private void ShowAllCategories()
        {
            AllCategoriesViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllCategoriesViewModel) as AllCategoriesViewModel;
    
            if (workspace == null)
            {
                workspace = new AllCategoriesViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllDiscounts()
        {
            AllDiscountsViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllDiscountsViewModel) 
                    as AllDiscountsViewModel;
    
            if (workspace == null)
            {
                workspace = new AllDiscountsViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        
        private void ShowAllPaymentMethods()
        {
            Console.WriteLine("ShowAllPaymentMethods called");
            AllPaymentMethodsViewModel workspace = 
                this.Workspaces.FirstOrDefault(vm => vm is AllPaymentMethodsViewModel) 
                    as AllPaymentMethodsViewModel;
    
            if (workspace == null)
            {
                Console.WriteLine("Creating new PaymentMethods workspace");
                workspace = new AllPaymentMethodsViewModel();
                this.Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }
        private void ShowAllOrderStatus()
        {
            AllOrderStatusViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllOrderStatusViewModel) 
                    as AllOrderStatusViewModel;
    
            if (workspace == null)
            {
                workspace = new AllOrderStatusViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        
        private void ShowAllManufacturers()
        {
            AllManufacturersViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllManufacturersViewModel) 
                    as AllManufacturersViewModel;
    
            if (workspace == null)
            {
                workspace = new AllManufacturersViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllCustomers()
        {
            AllCustomersViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllCustomersViewModel) 
                    as AllCustomersViewModel;
    
            if (workspace == null)
            {
                workspace = new AllCustomersViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllEmployees()
        {
            AllEmployeesViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllEmployeesViewModel) 
                    as AllEmployeesViewModel;
    
            if (workspace == null)
            {
                workspace = new AllEmployeesViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        
        private void ShowAllProducts()
        {
            AllProductsViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllProductsViewModel) 
                    as AllProductsViewModel;
    
            if (workspace == null)
            {
                workspace = new AllProductsViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        
        private void ShowAllOrders()
        {
            AllOrdersViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllOrdersViewModel) 
                    as AllOrdersViewModel;
    
            if (workspace == null)
            {
                workspace = new AllOrdersViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }

        private void ShowAllOrderPayments()
        {
            AllOrderPaymentsViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllOrderPaymentsViewModel) 
                    as AllOrderPaymentsViewModel;
    
            if (workspace == null)
            {
                workspace = new AllOrderPaymentsViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllReviews()
        {
            AllReviewsViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllReviewsViewModel) 
                    as AllReviewsViewModel;
    
            if (workspace == null)
            {
                workspace = new AllReviewsViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllUsers()
        {
            AllUsersViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllUsersViewModel) 
                    as AllUsersViewModel;
    
            if (workspace == null)
            {
                workspace = new AllUsersViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        
        private void ShowAllOrderDetails()
        {
            AllOrderDetailsViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllOrderDetailsViewModel) 
                    as AllOrderDetailsViewModel;
    
            if (workspace == null)
            {
                workspace = new AllOrderDetailsViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllProductImages()
        {
            AllProductImagesViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllProductImagesViewModel) 
                    as AllProductImagesViewModel;
    
            if (workspace == null)
            {
                workspace = new AllProductImagesViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        private void ShowAllDiscountProducts()
        {
            AllDiscountProductsViewModel workspace = 
                Workspaces.FirstOrDefault(vm => vm is AllDiscountProductsViewModel) 
                    as AllDiscountProductsViewModel;
    
            if (workspace == null)
            {
                workspace = new AllDiscountProductsViewModel();
                Workspaces.Add(workspace);
            }
    
            SetActiveWorkspace(workspace);
        }
        #endregion
        
        
        
        #region Private Helpers

        private void AddNewItem()
        {
            Console.WriteLine("AddNewItem called");
            if (ActiveWorkspace is AllRolesViewModel)
            {
                CreateView(new NewRoleViewModel());
            }
            else if (ActiveWorkspace is AllCategoriesViewModel)
            {
                CreateView(new NewCategoryViewModel());
            }
            else if (ActiveWorkspace is AllDiscountsViewModel)
            {
                CreateView(new NewDiscountViewModel());
            }
            else if (ActiveWorkspace is AllPaymentMethodsViewModel)
            {
                CreateView(new NewPaymentMethodViewModel());
            }
            else if (ActiveWorkspace is AllOrderStatusViewModel)
            {
                CreateView(new NewOrderStatusViewModel());
            }
            else if (ActiveWorkspace is AllManufacturersViewModel)
            {
                CreateView(new NewManufacturerViewModel());
            }
            else if (ActiveWorkspace is AllCustomersViewModel)
            {
                CreateView(new NewCustomerViewModel());
            }
            else if (ActiveWorkspace is AllEmployeesViewModel)
            {
                CreateView(new NewEmployeeViewModel());
            }
            else if (ActiveWorkspace is AllProductsViewModel)
            {
                CreateView(new NewProductViewModel());
            }
            else if (ActiveWorkspace is AllOrdersViewModel)
            {
                CreateView(new NewOrderViewModel());
            }
            else if (ActiveWorkspace is AllOrderPaymentsViewModel)
            {
                CreateView(new NewOrderPaymentViewModel());
            }
            else if (ActiveWorkspace is AllReviewsViewModel)
            {
                CreateView(new NewReviewViewModel());
            }
            else if (ActiveWorkspace is AllUsersViewModel)
            {
                CreateView(new NewUserViewModel());
            }
            else if (ActiveWorkspace is AllOrderDetailsViewModel)
            {
                CreateView(new NewOrderDetailViewModel());
            }
            else if (ActiveWorkspace is AllProductImagesViewModel)
            {
                CreateView(new NewProductImageViewModel());
            }
            else if (ActiveWorkspace is AllDiscountProductsViewModel)
            {
                CreateView(new NewDiscountProductViewModel());
            }
        }
        
        private void CreateView(BaseWorkspaceViewModel new_workspace)
        {
            this.Workspaces.Add(new_workspace); 
            this.SetActiveWorkspace(new_workspace); 
        }

        private void SetActiveWorkspace(BaseWorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }
        #endregion
    }    
}
