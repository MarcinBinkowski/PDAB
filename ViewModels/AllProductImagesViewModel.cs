using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllProductImagesViewModel : BaseDataViewModel<ProductImage>
{
    public AllProductImagesViewModel(IRepository<ProductImage> repository) 
        : base(repository, "ProductImages")
    {
    }
}
}