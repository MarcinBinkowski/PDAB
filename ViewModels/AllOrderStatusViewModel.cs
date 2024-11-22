using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllOrderStatusViewModel : AllEntitiesViewModel<OrderStatus>
    {
        public AllOrderStatusViewModel() : base("Order Status")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<OrderStatus>(
                dbContext.OrderStatuses.ToList()
            );
        }
    }
}