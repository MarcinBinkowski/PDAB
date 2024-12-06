using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllOrderDetailsViewModel : AllEntitiesViewModel<OrderDetail>
    {
        public AllOrderDetailsViewModel() : base("Order Details")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<OrderDetail>(
                dbContext.OrderDetails
                    .Include(od => od.Order)
                    .Include(od => od.Product)
                    .Include(od => od.Discount)
                    .ToList()
            );
        }
    }
}