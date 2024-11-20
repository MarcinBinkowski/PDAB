using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllProductsViewModel : BaseDataViewModel<Product>
{
    public AllProductsViewModel(IRepository<Product> repository) 
        : base(repository, "Products")
    {
    }
}
}