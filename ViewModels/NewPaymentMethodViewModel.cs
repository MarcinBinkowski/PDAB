using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewPaymentMethodViewModel : SingleEntityViewModel<PaymentMethod>
    {
        public NewPaymentMethodViewModel() : base("Payment Method")
        {
            item = new PaymentMethod();
        }

        public string MethodName
        {
            get => item.MethodName;
            set
            {
                item.MethodName = value;
                OnPropertyChanged(() => MethodName);
            }
        }

        public override void Save()
        {
            dbContext.PaymentMethods.Add(item);
            dbContext.SaveChanges();
        }
    }
}