using System.Collections.ObjectModel;
using PDAB.Models;

public class AllDiscountProductsViewModel : BaseDataViewModel<DiscountProduct>
{
    public AllDiscountProductsViewModel(IRepository<DiscountProduct> repository) 
        : base(repository, "Discount Products")
    {
    }
}