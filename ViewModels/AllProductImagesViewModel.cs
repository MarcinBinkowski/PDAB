using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;
using PDAB.ViewModels;

namespace PDAB.ViewModels
{
    public class AllProductImagesViewModel : AllEntitiesViewModel<ProductImage>
    {
        public AllProductImagesViewModel() : base("ProductImages")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<ProductImage>(
                dbContext.ProductImages
                    .Include(pi => pi.Product)
                    .ToList()
            );
        }
    }
}