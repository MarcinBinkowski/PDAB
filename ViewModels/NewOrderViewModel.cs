using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewOrderViewModel : SingleEntityViewModel<Order>
    {
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<OrderStatus> _orderStatuses;

        public NewOrderViewModel() : base("Order")
        {
            item = new Order 
            { 
                OrderDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            LoadRelatedData();
        }

        private void LoadRelatedData()
        {
            Customers = new ObservableCollection<Customer>(dbContext.Customers.ToList());
            OrderStatuses = new ObservableCollection<OrderStatus>(dbContext.OrderStatuses.ToList());
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged(() => Customers);
            }
        }

        public ObservableCollection<OrderStatus> OrderStatuses
        {
            get => _orderStatuses;
            set
            {
                _orderStatuses = value;
                OnPropertyChanged(() => OrderStatuses);
            }
        }

        public DateTime OrderDate
        {
            get => item.OrderDate;
            set
            {
                item.OrderDate = value;
                OnPropertyChanged(() => OrderDate);
            }
        }

        public decimal TotalAmount
        {
            get => item.TotalAmount;
            set
            {
                item.TotalAmount = value;
                OnPropertyChanged(() => TotalAmount);
            }
        }

        public int CustomerId
        {
            get => item.CustomerId;
            set
            {
                item.CustomerId = value;
                OnPropertyChanged(() => CustomerId);
            }
        }

        public int OrderStatusId
        {
            get => item.OrderStatusId;
            set
            {
                item.OrderStatusId = value;
                OnPropertyChanged(() => OrderStatusId);
            }
        }

        public override bool Save()
        {
            try
            {
                dbContext.Orders.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}