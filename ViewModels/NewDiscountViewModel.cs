using System.Windows;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewDiscountViewModel : SingleEntityViewModel<Discount>
    {
        public NewDiscountViewModel() : base("Discount")
        {
            item = new Discount();
        }

        public string DiscountName
        {
            get => item.DiscountName;
            set
            {
                item.DiscountName = value;
                OnPropertyChanged(() => DiscountName);
            }
        }

        public decimal DiscountPercentage
        {
            get => item.DiscountPercentage;
            set
            {
                item.DiscountPercentage = value;
                OnPropertyChanged(() => DiscountPercentage);
            }
        }

        public bool IsActive
        {
            get => item.IsActive;
            set
            {
                item.IsActive = value;
                OnPropertyChanged(() => IsActive);
            }
        }
        protected override bool ValidateBeforeSave()
        {
            if (item.DiscountPercentage < 0)
            {

                    MessageBox.Show("Discount Percentage cannot be negative.", 
                        "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;

            }
            if (item.DiscountPercentage > 100)
            {
                MessageBox.Show("Discount Percentage cannot be greater than 100.", 
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (item.DiscountPercentage == 0)
            {
                MessageBox.Show("Remember: Discount Percentage can be set to 0 only for testing purposes.", 
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return true;
        }
        public override bool Save()
        {
            try
            {
                dbContext.Discounts.Add(item);
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