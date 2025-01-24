using System.Collections.ObjectModel;
using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;
using PDAB.Repository;

namespace PDAB.ViewModels
{
    public class AddProductImageViewModel : BaseAddViewModel<ProductImage>
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly DialogService _dialogService;
        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public AddProductImageViewModel(IRepositoryFactory repositoryFactory, DialogService dialogService) 
            : base(repositoryFactory.GetRepository<ProductImage>(), "Add Product Image")
        {
            _repositoryFactory = repositoryFactory;
            _dialogService = dialogService;
            LoadProducts();
        }

        private async void LoadProducts()
        {
            Products = await _repositoryFactory.GetRepository<Product>().GetAllAsync();
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
    }
}