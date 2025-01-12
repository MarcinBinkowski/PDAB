using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
public class AllPaymentMethodsViewModel : BaseDataViewModel<PaymentMethod>
{
    public AllPaymentMethodsViewModel(IRepository<PaymentMethod> repository) 
        : base(repository, "PaymentMethods")
    {
    }
}
}