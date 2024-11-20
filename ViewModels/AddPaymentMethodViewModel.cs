using System.Windows.Input;
using PDAB.Helpers;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class AddPaymentMethodViewModel : BaseAddViewModel<PaymentMethod>
    {
        public AddPaymentMethodViewModel(IRepository<PaymentMethod> repository)
            : base(repository, "Add Payment Method")
        {
        }
    }
}