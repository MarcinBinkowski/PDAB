using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewEmployeeViewModel : SingleEntityViewModel<Employee>
    {
        public NewEmployeeViewModel() : base("Employee")
        {
            item = new Employee();
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

        public string Position
        {
            get => item.Position;
            set
            {
                item.Position = value;
                OnPropertyChanged(() => Position);
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
            if (!IsEmailValid(Email))
            {
                MessageBox.Show("Please enter a valid email address", 
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!IsPhoneValid(Phone))
            {
                MessageBox.Show("Phone number must be 7 to 12 digits long and can start with a plus sign", 
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
        private bool IsEmailValid(string email)
        {
            Regex emailValidationRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return !string.IsNullOrEmpty(email) && emailValidationRegex.IsMatch(email);
        }
        private bool IsPhoneValid(string phone)
        {
            Regex phoneValidationRegex = new(@"^\+?[0-9]{7,12}$");

            return !string.IsNullOrEmpty(phone) && phoneValidationRegex.IsMatch(phone);
        }
        
        public override bool Save()
        {
            try
            {
                dbContext.Employees.Add(item);
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