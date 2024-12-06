using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewDiscountProductViewModel : SingleEntityViewModel<DiscountProduct>
    {
        private ObservableCollection<Discount> _discounts;
        private ObservableCollection<Product> _products;

        public NewDiscountProductViewModel() : base("DiscountProduct")
        {
            item = new DiscountProduct();
            LoadCollections();
        }

        private void LoadCollections()
        {
            Discounts = new ObservableCollection<Discount>(dbContext.Discounts.ToList());
            Products = new ObservableCollection<Product>(dbContext.Products.ToList());
        }

        public ObservableCollection<Discount> Discounts
        {
            get => _discounts;
            set
            {
                _discounts = value;
                OnPropertyChanged(() => Discounts);
            }
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

        public int DiscountId
        {
            get => item.DiscountId;
            set
            {
                item.DiscountId = value;
                OnPropertyChanged(() => DiscountId);
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

        public override void Save()
        {
            dbContext.DiscountProducts.Add(item);
            dbContext.SaveChanges();
        }
    }
}