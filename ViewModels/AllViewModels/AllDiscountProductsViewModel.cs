
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllDiscountProductsViewModel : BaseDataViewModel<DiscountProduct>
    {
        public AllDiscountProductsViewModel(IRepository<DiscountProduct> repository) 
            : base(repository, "Discount Products")
        {
        }
    }
}
