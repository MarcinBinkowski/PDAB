using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllDiscountsViewModel : BaseDataViewModel<Discount>
    {
        public AllDiscountsViewModel(IRepository<Discount> repository) 
            : base(repository, "Discounts")
        {
        }
    }
}