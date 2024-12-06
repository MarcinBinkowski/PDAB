using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllDiscountProductsViewModel : AllEntitiesViewModel<DiscountProduct>
    {
        public AllDiscountProductsViewModel() : base("DiscountProducts")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<DiscountProduct>(
                dbContext.DiscountProducts
                    .Include(dp => dp.Discount)
                    .Include(dp => dp.Product)
                    .ToList()
            );
        }
    }
}