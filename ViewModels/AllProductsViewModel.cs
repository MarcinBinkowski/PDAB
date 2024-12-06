using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllProductsViewModel : AllEntitiesViewModel<Product>
    {
        public AllProductsViewModel() : base("Products")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Product>(
                dbContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .ToList()
            );
        }
    }
}