using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllOrdersViewModel : BaseDataViewModel<Order>
{
    public AllOrdersViewModel(IRepository<Order> repository) 
        : base(repository, "Orders")
    {
    }
}
}