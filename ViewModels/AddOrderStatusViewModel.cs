using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddOrderStatusViewModel : BaseAddViewModel<OrderStatus>
    {
        public AddOrderStatusViewModel(IRepository<OrderStatus> repository)
            : base(repository, "Add Order Status")
        {
        }
    }
}