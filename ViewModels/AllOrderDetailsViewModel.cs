using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllOrderDetailsViewModel : BaseDataViewModel<OrderDetail>
    {
        public AllOrderDetailsViewModel(IRepository<OrderDetail> repository) 
            : base(repository, "OrderDetails")
        {
        }
    }
}