using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllOrdersViewModel : AllEntitiesViewModel<Order>
    {
        public AllOrdersViewModel() : base("Orders")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<Order>(
                dbContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderStatus)
                    .ToList()
            );
        }
    }
}