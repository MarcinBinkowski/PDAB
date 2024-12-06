using System.Collections.ObjectModel;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewOrderPaymentViewModel : SingleEntityViewModel<OrderPayment>
    {
        private ObservableCollection<Order> _orders;
        private ObservableCollection<PaymentMethod> _paymentMethods;

        public NewOrderPaymentViewModel() : base("Order Payment")
        {
            item = new OrderPayment 
            { 
                PaymentDate = DateTime.Now
            };
            LoadRelatedData();
        }

        private void LoadRelatedData()
        {
            Orders = new ObservableCollection<Order>(dbContext.Orders.ToList());
            PaymentMethods = new ObservableCollection<PaymentMethod>(dbContext.PaymentMethods.ToList());
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(() => Orders);
            }
        }

        public ObservableCollection<PaymentMethod> PaymentMethods
        {
            get => _paymentMethods;
            set
            {
                _paymentMethods = value;
                OnPropertyChanged(() => PaymentMethods);
            }
        }

        public int OrderId
        {
            get => item.OrderId;
            set
            {
                item.OrderId = value;
                OnPropertyChanged(() => OrderId);
            }
        }

        public int PaymentMethodId
        {
            get => item.PaymentMethodId;
            set
            {
                item.PaymentMethodId = value;
                OnPropertyChanged(() => PaymentMethodId);
            }
        }

        public decimal Amount
        {
            get => item.Amount;
            set
            {
                item.Amount = value;
                OnPropertyChanged(() => Amount);
            }
        }

        public DateTime PaymentDate
        {
            get => item.PaymentDate;
            set
            {
                item.PaymentDate = value;
                OnPropertyChanged(() => PaymentDate);
            }
        }

        public override void Save()
        {
            dbContext.OrderPayments.Add(item);
            dbContext.SaveChanges();
        }
    }
}