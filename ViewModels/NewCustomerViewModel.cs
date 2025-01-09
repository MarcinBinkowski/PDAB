using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.EntityFrameworkCore;
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
        
        protected override bool ValidateBeforeSave()
        {
            
            if (!IsNameValid(FirstName))
            {
                MessageBox.Show("First name must be at least 2 characters long and contain only letters", 
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!IsNameValid(LastName))
            {
                MessageBox.Show("Last name must be at least 2 characters long and contain only letters", 
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
    
            return true;
        }

        private bool IsNameValid(string name)
        {
            Regex nameValidationRegex = new(@"^[a-zA-Z\s]{2,}$");

            return !string.IsNullOrEmpty(name) && nameValidationRegex.IsMatch(name);
        }

        public override bool Save()
        {
            try
            {
                dbContext.Customers.Add(item);
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