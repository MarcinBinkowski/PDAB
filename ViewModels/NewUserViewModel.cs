using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using PDAB.Models;

namespace PDAB.ViewModels
{
    public class NewUserViewModel : SingleEntityViewModel<User>
    {
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Role> _roles;
        private string _password;

        public NewUserViewModel() : base("User")
        {
            item = new User { IsActive = true };
            LoadRelatedData();
        }

        private void LoadRelatedData()
        {
            Employees = new ObservableCollection<Employee>(dbContext.Employees.ToList());
            Roles = new ObservableCollection<Role>(dbContext.Roles.ToList());
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(() => Employees);
            }
        }

        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged(() => Roles);
            }
        }

        public string Username
        {
            get => item.Username;
            set
            {
                item.Username = value;
                OnPropertyChanged(() => Username);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
            }
        }

        public int EmployeeId
        {
            get => item.EmployeeId;
            set
            {
                item.EmployeeId = value;
                OnPropertyChanged(() => EmployeeId);
            }
        }

        public int RoleId
        {
            get => item.RoleId;
            set
            {
                item.RoleId = value;
                OnPropertyChanged(() => RoleId);
            }
        }

        public bool IsActive
        {
            get => item.IsActive;
            set
            {
                item.IsActive = value;
                OnPropertyChanged(() => IsActive);
            }
        }

        public static byte[] HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty");

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public override bool Save()
        {
            try
            {
                dbContext.Users.Add(item);
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