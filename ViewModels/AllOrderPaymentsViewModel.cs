using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllOrderPaymentsViewModel : BaseDataViewModel<OrderPayment>
    {
        public AllOrderPaymentsViewModel(IRepository<OrderPayment> repository)
            : base(repository, "OrderPayments")
        {
        }
    }

}