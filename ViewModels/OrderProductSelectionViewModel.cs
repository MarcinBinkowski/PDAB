using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;
using PDAB.ViewModels;

namespace PDAB.ViewModels
{
    public class OrderProductSelectionViewModel : BusinessLogicViewModel
    {
        private ObservableCollection<ProductDisplayItem> _products;
        private decimal _totalValue;

        public ObservableCollection<ProductDisplayItem> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public decimal TotalValue
        {
            get => _totalValue;
            set
            {
                _totalValue = value;
                OnPropertyChanged(nameof(TotalValue));
            }
        }

        public OrderProductSelectionViewModel() : base()
        {
            Products = new ObservableCollection<ProductDisplayItem>();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            var products = await dbContext.Products
                .Include(p => p.ProductImages)
                .ToListAsync();

            foreach (var product in products)
            {
                Products.Add(new ProductDisplayItem
                {
                    Product = product,
                    Image = LoadImage(product.ProductImages.FirstOrDefault()?.ImageUrl)
                });
            }
        }

        private BitmapImage LoadImage(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(url);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
            catch
            {
                return null;
            }
        }
    }

    public class ProductDisplayItem : BaseViewModel
    {
        private int _quantity;
        public Product Product { get; set; }
        public BitmapImage Image { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
    }
}