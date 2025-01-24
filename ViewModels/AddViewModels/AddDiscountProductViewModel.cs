using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
    public class AddDiscountProductViewModel : BaseAddViewModel<DiscountProduct>
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly DialogService _dialogService;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Discount> _discounts;

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

        public AddDiscountProductViewModel(
            IRepositoryFactory repositoryFactory,
            DialogService dialogService) 
            : base(repositoryFactory.GetRepository<DiscountProduct>(), "Add Discount Product")
        {
            _repositoryFactory = repositoryFactory;
            _dialogService = dialogService;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                Products = await _repositoryFactory.GetRepository<Product>().GetAllAsync();
                Discounts = await _repositoryFactory.GetRepository<Discount>().GetAllAsync();
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error loading data: {ex.Message}", MessageBoxImage.Error);
            }
        }

        public ICommand SelectProductCommand => new BaseCommand(async () => 
        {
            var selected = await _dialogService.ShowSelectionDialog("Select Product", Products);
            Entity.Product = selected;
            Entity.ProductId = selected.ProductId;
            OnPropertyChanged(nameof(Entity));
        });

        public ICommand SelectDiscountCommand => new BaseCommand(async () => 
        {
            var selected = await _dialogService.ShowSelectionDialog("Select Discount", Discounts);
            Entity.Discount = selected;
            Entity.DiscountId = selected.DiscountId;
            OnPropertyChanged(nameof(Entity));
        });
    }
}