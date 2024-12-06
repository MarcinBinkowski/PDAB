using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllOrderPaymentsViewModel : AllEntitiesViewModel<OrderPayment>
    {
        public AllOrderPaymentsViewModel() : base("Order Payments")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<OrderPayment>(
                dbContext.OrderPayments
                    .Include(op => op.Order)
                    .Include(op => op.PaymentMethod)
                    .ToList()
            );
        }
    }
}