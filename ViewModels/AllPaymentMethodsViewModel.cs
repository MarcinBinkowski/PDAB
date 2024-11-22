using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AllPaymentMethodsViewModel : AllEntitiesViewModel<PaymentMethod>
    {
        public AllPaymentMethodsViewModel() : base("Payment Methods")
        {
            Load();
        }

        public override void Load()
        {
            List = new ObservableCollection<PaymentMethod>(
                dbContext.PaymentMethods.ToList()
            );
        }
    }
}