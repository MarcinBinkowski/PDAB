using GalaSoft.MvvmLight.Messaging;
using PDAB.Helpers;
using PDAB.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;


namespace PDAB.ViewModels
{
    public abstract class AllEntitiesViewModel<T> : BaseWorkspaceViewModel where T : class
    {
        #region DB
        protected readonly PdabDbContext dbContext;
        #endregion

        #region Command


        private BaseCommand _refreshCommand;
        public ICommand RefreshCommand // References in mainwindow.xaml
        {
            get
            {
                if (_refreshCommand == null)
                    _refreshCommand = new BaseCommand(() => Load()); // Load is implemented on all ViewModels
                return _refreshCommand;
            }
        }
        private BaseCommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                Console.WriteLine("DeleteCommand");
                if (_deleteCommand == null)
                    _deleteCommand = new BaseCommand(
                        () => DeleteSelected(),
                        () => SelectedItem != null);
                return _deleteCommand;
            }
        }

        
        #endregion

        #region List
        private ObservableCollection<T> _List;
        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);
            }
        }
        #endregion

        #region Constructor
        public AllEntitiesViewModel(String displayName)
        {
            dbContext = new PdabDbContext();
            base.DisplayName = displayName;
        }
        #endregion

        #region Helpers
        public abstract void Load();

        protected T _selectedItem; // set property on every AllView
        public T SelectedItem 
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                Console.WriteLine($"SelectedItem changed to: {value}");
                OnPropertyChanged(() => SelectedItem);
                (_deleteCommand as BaseCommand)?.RaiseCanExecuteChanged();
            }
        }
        private void DeleteSelected()
        {
            if (SelectedItem == null) return;

            try
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this item?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (typeof(T) == typeof(Discount))
                    {
                        var discount = SelectedItem as Discount;
                        var hasRelatedOrders = dbContext.OrderDetails
                            .Any(od => od.DiscountId == discount.DiscountId);

                        if (hasRelatedOrders)
                        {
                            MessageBox.Show(
                                "Cannot delete this discount because it is used in orders.\nRemove related orders first.",
                                "Delete Failed",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return;
                        }
                    }

                    dbContext.Set<T>().Remove(SelectedItem);
                    dbContext.SaveChanges();
                    Load();
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show(
                    "Cannot delete this item because it is referenced by other records.",
                    "Delete Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            // Console.WriteLine("DeleteSelected called");
            // if (SelectedItem == null) return;
            //
            // var result = MessageBox.Show(
            //     "Are you sure you want to delete this item?",
            //     "Confirm Delete",
            //     MessageBoxButton.YesNo,
            //     MessageBoxImage.Warning);
            //
            // if (result == MessageBoxResult.Yes)
            // {
            //     dbContext.Set<T>().Remove(SelectedItem);
            //     dbContext.SaveChanges();
            //     Load();
            // }
        }
        #endregion
    }
}