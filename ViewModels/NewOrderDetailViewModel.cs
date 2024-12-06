using System.Collections.ObjectModel;
using System.Windows;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewOrderDetailViewModel : SingleEntityViewModel<OrderDetail>
    {
        private ObservableCollection<Order> _orders;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Discount> _discounts;

        public NewOrderDetailViewModel() : base("Order Detail")
        {
            item = new OrderDetail();
            LoadRelatedData();
        }

        private void LoadRelatedData()
        {
            Orders = new ObservableCollection<Order>(dbContext.Orders.ToList());
            Products = new ObservableCollection<Product>(dbContext.Products.ToList());
            Discounts = new ObservableCollection<Discount>(dbContext.Discounts.ToList());
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(() => Orders);
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

        public ObservableCollection<Discount> Discounts
        {
            get => _discounts;
            set
            {
                _discounts = value;
                OnPropertyChanged(() => Discounts);
            }
        }

        public int OrderId
        {
            get => item.OrderId;
            set
            {
                item.OrderId = value;
                OnPropertyChanged(() => OrderId);
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

        public int Quantity
        {
            get => item.Quantity;
            set
            {
                item.Quantity = value;
                OnPropertyChanged(() => Quantity);
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

        public int DiscountId
        {
            get => item.DiscountId;
            set
            {
                item.DiscountId = value;
                OnPropertyChanged(() => DiscountId);
            }
        }

        public override void Save()
        {
            if (Quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0", "Validation Error");
                return;
            }

            if (UnitPrice <= 0)
            {
                MessageBox.Show("Unit price must be greater than 0", "Validation Error");
                return;
            }

            dbContext.OrderDetails.Add(item);
            dbContext.SaveChanges();
        }
    }
}