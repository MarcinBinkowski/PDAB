using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewReviewViewModel : SingleEntityViewModel<Review>
    {
        private ObservableCollection<Product> _products;
        private ObservableCollection<Customer> _customers;

        public NewReviewViewModel() : base("Review")
        {
            item = new Review 
            { 
                ReviewDate = DateTime.Now,
                Rating = 1
            };
            LoadRelatedData();
        }

        private void LoadRelatedData()
        {
            Products = new ObservableCollection<Product>(dbContext.Products.ToList());
            Customers = new ObservableCollection<Customer>(dbContext.Customers.ToList());
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

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(() => Customers);
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

        public int CustomerId
        {
            get => item.CustomerId;
            set
            {
                item.CustomerId = value;
                OnPropertyChanged(() => CustomerId);
            }
        }

        public byte Rating
        {
            get => item.Rating;
            set
            {
                if (value >= 1 && value <= 5)
                {
                    item.Rating = value;
                    OnPropertyChanged(() => Rating);
                }
            }
        }

        public string Comment
        {
            get => item.Comment ?? string.Empty;
            set
            {
                item.Comment = value;
                OnPropertyChanged(() => Comment);
            }
        }

        public DateTime ReviewDate
        {
            get => item.ReviewDate;
            set
            {
                item.ReviewDate = value;
                OnPropertyChanged(() => ReviewDate);
            }
        }
        
        protected override bool ValidateBeforeSave()
        {

            if (ReviewDate > DateTime.Now)
            {
                MessageBox.Show(
                    "Review date cannot be in the future.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Comment) && Comment.Length < 5)
            {
                MessageBox.Show(
                    "Comment must be at least 5 characters or null.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            return true;
        }

        public override bool Save()
        {
            try
            {
                dbContext.Reviews.Add(item);
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