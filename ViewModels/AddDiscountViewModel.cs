using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddDiscountViewModel : BaseAddViewModel<Discount>
    {
        public AddDiscountViewModel(IRepository<Discount> repository) 
            : base(repository, "Add Discount")
        {
        }
    }
}
