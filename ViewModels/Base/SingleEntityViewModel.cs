using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.ViewModels;

public abstract class SingleEntityViewModel<T> : BaseWorkspaceViewModel
{
    #region DB
    protected PdabDbContext dbContext;
    #endregion

    #region Item
    protected T item;
    #endregion

    #region Command
    private BaseCommand _SaveCommand;
    public ICommand SaveCommand
    {
        get
        {
            if (_SaveCommand == null)
                _SaveCommand = new BaseCommand(() => SaveAndClose());
            return _SaveCommand;
        }
    }
    #endregion

    #region Constructor
    public SingleEntityViewModel(string displayName)
    {
        base.DisplayName = displayName;
        dbContext = new PdabDbContext();
    }
    #endregion

    #region Helpers
    public abstract bool Save();
    protected virtual bool ValidateBeforeSave()
    {
        return true;
    }


    public void SaveAndClose()
    {
        if (ValidateBeforeSave())
        {
            if(!Save())
            {
                MessageBox.Show("Error saving entity. Please check all required fields.",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var mainVM = Application.Current.MainWindow.DataContext as MainWindowViewModel;
            if (this is NewRoleViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllRolesViewModel) as AllRolesViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewCategoryViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllCategoriesViewModel) as AllCategoriesViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewCustomerViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllCustomersViewModel) as AllCustomersViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewDiscountViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllDiscountsViewModel) as AllDiscountsViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewEmployeeViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllEmployeesViewModel) as AllEmployeesViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewOrderViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllOrdersViewModel) as AllOrdersViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewOrderDetailViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllOrderDetailsViewModel) as AllOrderDetailsViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewOrderStatusViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllOrderStatusViewModel) as AllOrderStatusViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewPaymentMethodViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllPaymentMethodsViewModel) as AllPaymentMethodsViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewProductImageViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllProductImagesViewModel) as AllProductImagesViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewProductViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllProductsViewModel) as AllProductsViewModel;
                parentWorkspace?.Load();
            }
            else if (this is NewDiscountProductViewModel)
            {
                var parentWorkspace = mainVM?.Workspaces
                    .FirstOrDefault(w => w is AllDiscountProductsViewModel) as AllDiscountProductsViewModel;
                parentWorkspace?.Load();
            }

            base.OnRequestClose();
        }
    }
    #endregion
}