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