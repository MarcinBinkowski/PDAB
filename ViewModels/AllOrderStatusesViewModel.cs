using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllOrderStatusesViewModel : BaseDataViewModel<OrderStatus>
{
    public AllOrderStatusesViewModel(IRepository<OrderStatus> repository) 
        : base(repository, "OrderStatuses")
    {
    }
}
}