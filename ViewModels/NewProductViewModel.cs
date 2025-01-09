using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewProductViewModel : SingleEntityViewModel<Product>
    {
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Manufacturer> _manufacturers;

        public NewProductViewModel() : base("Product")
        {
            item = new Product();
            LoadCategories();
            LoadManufacturers();
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(() => Categories);
            }
        }

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get => _manufacturers;
            set
            {
                _manufacturers = value;
                OnPropertyChanged(() => Manufacturers);
            }
        }

        public string ProductName
        {
            get => item.ProductName;
            set
            {
                item.ProductName = value;
                OnPropertyChanged(() => ProductName);
            }
        }

        public decimal UnitPrice
        {
            get => item.UnitPrice;
            set
            {
                item.UnitPrice = value;
                OnPropertyChanged(() => UnitPrice);
            }
        }

        public int StockQuantity
        {
            get => item.StockQuantity;
            set
            {
                item.StockQuantity = value;
                OnPropertyChanged(() => StockQuantity);
            }
        }

        public string Description
        {
            get => item.Description;
            set
            {
                item.Description = value;
                OnPropertyChanged(() => Description);
            }
        }

        public int CategoryId
        {
            get => item.CategoryId;
            set
            {
                item.CategoryId = value;
                OnPropertyChanged(() => CategoryId);
            }
        }

        public int ManufacturerId
        {
            get => item.ManufacturerId;
            set
            {
                item.ManufacturerId = value;
                OnPropertyChanged(() => ManufacturerId);
            }
        }

        private void LoadCategories()
        {
            Categories = new ObservableCollection<Category>(dbContext.Categories.ToList());
        }

        private void LoadManufacturers()
        {
            Manufacturers = new ObservableCollection<Manufacturer>(dbContext.Manufacturers.ToList());
        }

        public override bool Save()
        {
            try
            {
                dbContext.Products.Add(item);
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