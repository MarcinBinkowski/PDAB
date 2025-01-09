using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewProductImageViewModel : SingleEntityViewModel<ProductImage>
    {
        private ObservableCollection<Product> _products;

        public NewProductImageViewModel() : base("ProductImage")
        {
            item = new ProductImage();
            LoadProducts();
        }

        private void LoadProducts()
        {
            Products = new ObservableCollection<Product>(dbContext.Products.ToList());
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(() => Products);
            }
        }

        public int ProductId
        {
            get => item.ProductId;
            set
            {
                item.ProductId = value;
                OnPropertyChanged(() => ProductId);
            }
        }

        public string ImageUrl
        {
            get => item.ImageUrl;
            set
            {
                item.ImageUrl = value;
                OnPropertyChanged(() => ImageUrl);
            }
        }

        public override bool Save()
        {
            try
            {
                dbContext.ProductImages.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}