using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewCustomerViewModel : SingleEntityViewModel<Customer>
    {
        public NewCustomerViewModel() : base("Customer")
        {
            item = new Customer();
        }

        public string FirstName
        {
            get => item.FirstName;
            set
            {
                item.FirstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get => item.LastName;
            set
            {
                item.LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }

        public string Email
        {
            get => item.Email;
            set
            {
                item.Email = value;
                OnPropertyChanged(() => Email);
            }
        }

        public string Address
        {
            get => item.Address;
            set
            {
                item.Address = value;
                OnPropertyChanged(() => Address);
            }
        }

        public string Phone
        {
            get => item.Phone;
            set
            {
                item.Phone = value;
                OnPropertyChanged(() => Phone);
            }
        }

        public override void Save()
        {
            dbContext.Customers.Add(item);
            dbContext.SaveChanges();
        }
    }
}