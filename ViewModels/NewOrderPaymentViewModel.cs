using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
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

        protected override bool ValidateBeforeSave()
        {
            if (Amount <= 0)
            {
                MessageBox.Show(
                    "Amount must be greater than 0.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }

            if (PaymentDate > DateTime.Now)
            {
                MessageBox.Show(
                    "Payment Date cannot be in the future.",
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return false;
            }
            Console.WriteLine($"Saving OrderPayment: OrderId={OrderId}, " + 
                              $"PaymentMethodId={PaymentMethodId}, " +
                              $"Amount={Amount}, " +
                              $"PaymentDate={PaymentDate}");

            return true;
        }
        
        public override bool Save()
        {
            try
            {
                item = new OrderPayment
                {
                    OrderId = OrderId,
                    PaymentMethodId = PaymentMethodId,
                    Amount = Amount,
                    PaymentDate = PaymentDate
                };
                dbContext.OrderPayments.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException: " + ex.InnerException.Message);
                }
                Console.WriteLine($"Error saving OrderPayment: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}